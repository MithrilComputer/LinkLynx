using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Src.Core.Utility.Signals.Attributes;
using LinkLynx.Core.Utility.Dispatchers;
using LinkLynx.Core.Utility.Registries;
using System;
using System.Linq;
using System.Reflection;

namespace LinkLynx.Core.Engine
{
    /// <summary>
    /// A scanner Class made to look for all the pages in a program and register them, and their logic joins.
    /// </summary>
    internal static class PageScanner
    {
        /// <summary>
        /// Runs the scanner, finds all the pages by attribute, then registers them, and their logic joins.
        /// </summary>
        internal static void Run()
        {

            // This might need to change at some point, 
            // Only affects startup time, but could get heavy later if I don't optimize it with a few hundred pages in mind.
            // Not to mention I don't like how it looks for some reason.

            Type baseType = typeof(PageLogicBase); // Cache

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;

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

                    if (type.IsClass)
                    {
                        PageAttribute pageAttribute = (PageAttribute)type.GetCustomAttribute(typeof(PageAttribute), inherit: false);

                        if (pageAttribute == null)
                            continue;

                        ushort pageId = pageAttribute.Id;

                        // Put the page factory in the registry
                        LinkLynxServices.pageRegistry.RegisterPage(pageId, panel => (PageLogicBase)Activator.CreateInstance(type, new object[] { panel }));

                        // Auto-wire joins to the registrar
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
            MethodInfo method = typeof(AutoJoinRegistrar)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == "RegisterJoins" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(ushort));

            if (method == null)
                throw new MissingMethodException("AutoJoinRegistrar.RegisterJoins<T>(ushort) not found.");

            method.MakeGenericMethod(pageType).Invoke(null, new object[] { pageId });
        }

        /// <summary>
        /// Attempts to register a enum marked with SigType to the EnumSignalTypeRegistry.
        /// </summary>
        /// <param name="type">This should be a type of enum, but wrapped by the type class.</param>
        private static void TryRegisterEnumSigType(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(SigTypeAttribute), inherit: false);

            if (attributes.Length == 0) return;
            if (attributes.Length > 1)
                throw new InvalidOperationException($"[PageScanner] Error: Multiple [SigType] on enum {type.FullName}.");

            eSigType sig = ((SigTypeAttribute)attributes[0]).JoinType;

            LinkLynxServices.enumSignalTypeRegistry.Register(type, sig);
        }
    }
}
