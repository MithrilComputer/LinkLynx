using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Signals.Attributes;
using LinkLynx.Core.Utility.Dispatchers;
using System;
using System.Linq;
using System.Reflection;
using LinkLynx.Core.Utility.Debugging.Logging;

namespace LinkLynx.Core.Engine
{
    /// <summary>
    /// A scanner Class made to look for all the pages in a program and register them, and their logic joins.
    /// </summary>
    internal static class PageScanner
    {
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Runs the scanner, finds all the pages by attribute, then registers them, and their logic joins.
        /// </summary>
        internal static void Run()
        {
            ConsoleLogger.Log("[PageScanner] Scanning for pages...");
            // This might need to change at some point, 
            // Only affects startup time, but could get heavy later if I don't optimize it with a few hundred pages in mind.
            // Not to mention I don't like how it looks for some reason.

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
                            ConsoleLogger.Log($"[PageScanner] Scanned Class has no attributes, skipping {name}");
                            continue;
                        }

                        ConsoleLogger.Log($"[PageScanner] Scanned Page has attributes, processing {name}");

                        ushort pageId = pageAttribute.Id;

                        // Put the page factory in the registry
                        LinkLynxServices.pageRegistry.RegisterPage(pageId, panel => (PageLogicBase)Activator.CreateInstance(type, new object[] { panel }));

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
        private static void AutoWireJoins(Type pageType, ushort pageId)
        {
            ConsoleLogger.Log($"[PageScanner] Attempting to wire page class {pageType.FullName}'s signal joins...");

            MethodInfo method = typeof(AutoJoinRegistrar)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == "RegisterJoins" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(ushort));

            if (method == null)
                throw new MissingMethodException("AutoJoinRegistrar.RegisterJoins<T>(ushort) not found.");

            ConsoleLogger.Log("[PageScanner] Registering Joins...");

            method.MakeGenericMethod(pageType).Invoke(null, new object[] { pageId });
        }

        /// <summary>
        /// Attempts to register a enum marked with SigType to the EnumSignalTypeRegistry.
        /// </summary>
        /// <param name="type">This should be a type of enum, but wrapped by the type class.</param>
        private static void TryRegisterEnumSigType(Type type)
        {
            ConsoleLogger.Log($"[PageScanner] Found Enum '{type.FullName}' Attempting to register...");

            object[] attributes = type.GetCustomAttributes(typeof(SigTypeAttribute), inherit: false);

            if (attributes.Length == 0) 
            {
                ConsoleLogger.Log($"[PageScanner] Enum '{type.FullName}' Has no attributes, skipping.");
                return;
            }

            if (attributes.Length > 1)
            {
                ConsoleLogger.Log($"[PageScanner] Enum '{type.FullName}' Has too many attributes, skipping.");
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
                if (assembly.IsDynamic)
                    return false;

                string assemblyPath = assembly.Location;

                if (!String.IsNullOrEmpty(assemblyPath) && assemblyPath.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleLogger.Log($"[PageScanner] Assembly '{assembly.FullName}' is in the base directory, processing.");
                    return true;
                }

                return false;

            } catch
            {
                return false;
            }
        }
    }
}
