using System;
using System.Reflection;
using LinkLynx.Core.Logic;
using LinkLynx.Core.Utility.Dispatchers;
using LinkLynx.Core.Utility.Registries;
using LinkLynx.Core.Utility.Signals;

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
            Type baseType = typeof(PageLogicBase);

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
                    if (type.IsAbstract || !baseType.IsAssignableFrom(type)) 
                        continue;

                    PageAttribute pageAttribute = type.GetCustomAttribute<PageAttribute>();

                    if (pageAttribute == null) 
                        continue;

                    ushort pageId = pageAttribute.Id;

                    // Put the page factory in the registry
                    PageRegistry.RegisterPage(pageId, panel =>
                        (PageLogicBase)Activator.CreateInstance(type, new object[] { panel }));

                    // Auto-wire joins to the registrar
                    AutoWireJoins(type, pageId);
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
            typeof(AutoJoinRegistrar)
                .GetMethod("RegisterJoins")
                .MakeGenericMethod(pageType)
                .Invoke(null, new object[] { pageId });
        }
    }
}
