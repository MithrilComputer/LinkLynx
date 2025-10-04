using Crestron.SimplSharpPro;
using LinkLynx.Core.Attributes;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Src.Core.Interfaces.Utility.Dispatching;
using System;
using System.Linq;
using System.Reflection;

namespace LinkLynx.Wiring.Engine
{
    /// <summary>
    /// A scanner Class made to look for all the pages in a program and register them, and their logic joins.
    /// </summary>
    internal sealed class AutoRegisterScanner : IAutoJoinRegistrar
    {
        private readonly ILogger consoleLogger;
        private readonly IPageRegistry pageRegistry;
        private readonly 

        public AutoRegisterScanner(ILogger consoleLogger)
        {
            this.consoleLogger = consoleLogger;
        }

        /// <summary>
        /// Runs the scanner, finds all the pages by attribute, then registers them, and their logic joins.
        /// </summary>
        public void Run()
        {
            consoleLogger.Log("[AutoRegisterScanner] Page Scanner started! Scanning...");

            Type baseType = typeof(PageLogicBase); // Cache

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!CheckIfWhitelisted(assembly))
                {
                    continue;
                }

                Type[] types;

                string name = assembly.GetName().Name;

                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (Type type in types)
                {
                    if (type.IsEnum)
                    {
                        TryRegisterEnumSigType(type);
                    }
                }

                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        PageAttribute pageAttribute = (PageAttribute)type.GetCustomAttribute(typeof(PageAttribute), inherit: false);

                        if (pageAttribute == null)
                        {
                            consoleLogger.Log($"[AutoRegisterScanner] Scanned Class has no attributes, skipping {type.FullName}");
                            continue;
                        }

                        consoleLogger.Log($"[AutoRegisterScanner] Scanned Page has attributes, processing {type.FullName}");

                        ushort pageId = pageAttribute.Id;

                        // Put the page factory in the registry
                        pageRegistry.RegisterPage(pageId, panel => (PageLogicBase)Activator.CreateInstance(type, new object[] { panel }));

                        // Auto wire the joins to the registrar
                        AutoWireJoins(type, pageId);
                    }
                }
            }
        }

        /// <summary>
        /// Registers the joins to the global registry via the AutoJoinRegistrar. 
        /// Used to get around the problem of using the Type as a var.
        /// </summary>
        /// <param name="pageType">The type to be added to the register.</param>
        /// <param name="pageId">The id of the given type.</param>
        private void AutoWireJoins(Type pageType, ushort pageId)
        {
            consoleLogger.Log($"[AutoRegisterScanner] Attempting to wire page class {pageType.FullName}'s signal joins...");

            MethodInfo method = typeof(AutoJoinRegistrar)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == "RegisterJoins" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(ushort));

            if (method == null)
                throw new MissingMethodException("AutoJoinRegistrar.RegisterJoins<T>(ushort) not found.");

            consoleLogger.Log("[AutoRegisterScanner] Registering Joins...");

            method.MakeGenericMethod(pageType).Invoke(null, new object[] { pageId });
        }

        /// <summary>
        /// Attempts to register a enum marked with SigType to the EnumSignalTypeRegistry.
        /// </summary>
        /// <param name="type">This should be a type of enum, but wrapped by the type class.</param>
        private static void TryRegisterEnumSigType(Type type)
        {
            consoleLogger.Log($"[AutoRegisterScanner] Found Enum '{type.FullName}' Attempting to register...");

            object[] attributes = type.GetCustomAttributes(typeof(SigTypeAttribute), inherit: false);

            if (attributes.Length == 0) 
            {
                consoleLogger.Log($"[AutoRegisterScanner] Enum '{type.FullName}' Has no attributes, skipping.");
                return;
            }

            if (attributes.Length > 1)
            {
                consoleLogger.Log($"[AutoRegisterScanner] Enum '{type.FullName}' Has too many attributes, skipping.");
                return;
            }

            eSigType sig = ((SigTypeAttribute)attributes[0]).JoinType;

            LinkLynxServices.enumSignalTypeRegistry.Register(type, sig);
        }

        /// <summary>
        /// Checks if the assembly is whitelisted to be scanned.
        /// </summary>
        private static bool CheckIfWhitelisted(Assembly assembly)
        {
            try
            {
                return assembly.GetReferencedAssemblies().Any(r =>
                        r.Name.StartsWith("LinkLynx.", StringComparison.Ordinal) ||
                        r.Name.Equals("LinkLynx", StringComparison.Ordinal));
            }
            catch
            {
                return false; // defensive for weird loaders
            }
        }
    }
}
