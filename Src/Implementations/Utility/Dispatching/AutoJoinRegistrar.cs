using LinkLynx.Core.Attributes;
using LinkLynx.Core.Interfaces.Collections.Registries;
using LinkLynx.Core.Interfaces.Utility.Debugging.Logging;
using LinkLynx.Core.Interfaces.Utility.Dispatching;
using LinkLynx.Core.Logic.Pages;
using LinkLynx.Core.CrestronPOCOs;
using System.Reflection;

namespace LinkLynx.Implementations.Utility.Dispatching
{
    /// <summary>
    /// Responsible for automatically registering all joins at runtime.
    /// Uses the [Join(Enum)] attribute to assign bindings to methods
    /// </summary>
    public class AutoJoinRegistrar : IAutoJoinRegistrar
    {
        private readonly ILogger consoleLogger;
        private readonly IJoinDispatcher dispatcherHelper;
        private readonly IReversePageRegistry reversePageRegistry;

        /// <summary>
        /// The constructor for the AutoJoinRegistrar.
        /// </summary>
        public AutoJoinRegistrar(ILogger consoleLogger, IJoinDispatcher dispatcherHelper, IReversePageRegistry reversePageRegistry)
        {
            this.consoleLogger = consoleLogger;
            this.dispatcherHelper = dispatcherHelper;
            this.reversePageRegistry = reversePageRegistry;
        }

        /// <summary>
        /// This registers all the joins on a page automatically by going through the page's method attributes.
        /// </summary>
        /// <param name="pageId">The id of the given page.</param>
        public void RegisterJoins<T>(ushort pageId) where T : PageLogicScript
        {
            Type logicType = typeof(T);

            try
            {
                foreach (MethodInfo method in logicType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    JoinAttribute[] joinAttributes = method.GetCustomAttributes(typeof(JoinAttribute), false) as JoinAttribute[];

                    if (joinAttributes == null || joinAttributes.Length == 0)
                        continue;

                    consoleLogger.Log($"[AutoJoinRegistrar] Found {joinAttributes.Length} Join Attributes on method '{method.Name}'");

                    foreach (JoinAttribute joinAttr in joinAttributes)
                    {
                        if (joinAttr.Join is Enum joinEnum)
                        {
                            consoleLogger.Log($"[AutoJoinRegistrar] Registering Join '{joinEnum}' on method '{method.Name}'");

                            if(!reversePageRegistry.TryRegister(joinEnum, pageId))
                            {
                                consoleLogger.Log($"[AutoJoinRegistrar] Warning: Join '{joinEnum}' is already registered to another page. Skipping registration on method '{method.Name}'. Skipping...");

                                continue;
                            }

                            dispatcherHelper.AddToDispatcher(joinEnum, BuildLambda(method));
                        }
                        else
                        {
                            consoleLogger.Log($"[AutoJoinRegistrar] Warning: Join attribute on method '{method.Name}' does not contain a valid Enum.");
                        }
                    }
                }
            }
            catch
            (Exception ex)
            {
                consoleLogger.Log($"[AutoJoinRegistrar] Error while registering joins for page ID {pageId}: {ex.Message}");
            }
        }

        /// <summary>
        /// A method that generates a Lambda Action to be added to the dispatcher.
        /// </summary>
        /// <param name="method">The method to be converted to the Lambda Action</param>
        private Action<PageLogicScript, SignalEventData> BuildLambda(MethodInfo method)
        {
            return (page, args) =>
            {
                if (page == null)
                {
                    consoleLogger.Log($"[AutoJoinRegistrar] Error: Page instance was null.");
                    return;
                }

                try
                {
                    method.Invoke(page, new object[] { args });
                }
                catch (TargetInvocationException ex)
                {
                    consoleLogger.Log($"[AutoJoinRegistrar] Error in method '{method.Name}': {ex.InnerException?.Message ?? "Unknown inner exception"}");
                    consoleLogger.Log(ex.InnerException?.StackTrace ?? "No stack trace");
                }
                catch (Exception ex)
                {
                    consoleLogger.Log($"[AutoJoinRegistrar] Error: {ex.Message}");
                }
            };
        }
    }
}