using Crestron.SimplSharpPro;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.Utility.Registries;
using LinkLynx.Core.Utility.Debugging.Logging;
using System;
using System.Reflection;
using LinkLynx.Core.Src.Core.Utility.Signals.Attributes;

namespace LinkLynx.Core.Utility.Dispatchers
{
    /// <summary>
    /// Responsible for automatically registering all joins at runtime. 
    /// Uses the [Join(Enum)] attribute to assign bindings to methods
    /// </summary>
    internal static class AutoJoinRegistrar
    {
        /// <summary>
        /// This registers all the joins on a page automatically by going through the page's method attributes.
        /// </summary>
        /// <param name="pageId">The id of the given page.</param>
        internal static void RegisterJoins<T>(ushort pageId) where T : PageLogicBase
        {
            Type logicType = typeof(T);

            foreach (MethodInfo method in logicType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                JoinAttribute[] joinAttributes = method.GetCustomAttributes(typeof(JoinAttribute), false) as JoinAttribute[];

                if (joinAttributes == null || joinAttributes.Length == 0)
                    continue;

                foreach (JoinAttribute joinAttr in joinAttributes)
                {
                    if (joinAttr.Join is Enum joinEnum)
                    {
                        LinkLynxServices.reversePageRegistry.RegisterPageKeyFromJoin(joinEnum, pageId);

                        DispatcherHelper.AddToDispatcher(joinEnum, BuildLambda<T>(method));
                    }
                    else
                    {
                        ConsoleLogger.Log($"[AutoJoinRegistrar] Warning: Join attribute on method '{method.Name}' does not contain a valid Enum.");
                    }
                }
            }
        }

        /// <summary>
        /// A method that generates a Lambda Action to be added to the dispatcher.
        /// </summary>
        /// <param name="method">The method to be converted to the Lambda Action</param>
        private static Action<PageLogicBase, SigEventArgs> BuildLambda<T>(MethodInfo method) where T : PageLogicBase
        {
            return (page, args) =>
            {
                if (page == null)
                {
                    ConsoleLogger.Log($"[AutoJoinRegistrar] Error: Page instance was null.");
                    return;
                }

                try
                {
                    method.Invoke(page, new object[] { args });
                }
                catch (TargetInvocationException ex)
                {
                    ConsoleLogger.Log($"[AutoJoinRegistrar] Error in method '{method.Name}': {ex.InnerException?.Message ?? "Unknown inner exception"}");
                    ConsoleLogger.Log(ex.InnerException?.StackTrace ?? "No stack trace");
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Log($"[AutoJoinRegistrar] Error: {ex.Message}");
                }
            };
        }
    }
}
