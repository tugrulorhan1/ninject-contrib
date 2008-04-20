using System;
using System.Reflection;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using Ninject.Core.Planning;
using Ninject.Extensions.MessageBroker;
using Ninject.Extensions.MessageBroker.Infrastructure;
using Ninject.Framework.PresentationFoundation.Infrastructure;
using Ninject.Framework.PresentationFoundation.Infrastructure.ExtensionMethods;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// A planning strategy that examines types via reflection to determine if there are any
    /// message publications or subscriptions defined.
    /// </summary>
    public class WpfEventReflectionStrategy : EventReflectionStrategy
    {
        #region Public Methods
        /// <summary>
        /// Executed to build the activation plan.
        /// </summary>
        /// <param name="binding">The binding that points at the type whose activation plan is being released.</param>
        /// <param name="type">The type whose activation plan is being manipulated.</param>
        /// <param name="plan">The activation plan that is being manipulated.</param>
        /// <returns>
        /// A value indicating whether to proceed or interrupt the strategy chain.
        /// </returns>
        public override StrategyResult Build(IBinding binding, Type type, IActivationPlan plan)
        {
            base.Build(binding, type, plan);
            var events = type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var evt in events)
            {
                var ensureAction = false;
                var attributes = AttributeReader.GetAll<PublishActionAttribute>(evt);
                foreach (var attribute in attributes)
                {
                    if (ensureAction)
                        Ensure.ArgumentNotNullOrEmptyString(attribute.Action, "action");

                    string channel;
                    if (string.IsNullOrEmpty(attribute.Action))
                    {
                        channel = evt.ChannelNameFor(type);
                        ensureAction = true;
                    }
                    else channel = attribute.ChannelNameFor(type);

                    plan.Directives.Add(CreatePublicationDirective(channel, evt));
                }
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var attributes = AttributeReader.GetAll<SubscribeToActionAttribute>(method);
                foreach (var attribute in attributes)
                {
                    var channel = attribute.ChannelName();
                    plan.Directives.Add(CreateSubscriptionDirective(channel, method));
                }
            }

            return StrategyResult.Proceed;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a new directive representing a message publication.
        /// </summary>
        /// <param name="channel">The channel to publish to.</param>
        /// <param name="evt">The event to publish.</param>
        /// <returns>
        /// A <see cref="PublicationDirective"/> representing the publication.
        /// </returns>
        private static PublicationDirective CreatePublicationDirective(string channel, EventInfo evt)
        {
            return new PublicationDirective(channel, evt);
        }

        

        /// <summary>
        /// Creates a new directive representing a message subscription.
        /// </summary>
        /// <param name="channel">The channel to subscribe to.</param>
        /// <param name="method">The method that will receive messages from the channel.</param>
        /// <returns>
        /// A <see cref="SubscriptionDirective"/> representing the publication.
        /// </returns>
        private SubscriptionDirective CreateSubscriptionDirective(string channel, MethodInfo method)
        {
            var injectorFactory = Kernel.GetComponent<IInjectorFactory>() as WpfDynamicInjectorFactory;

            if(injectorFactory == null)
                throw new NullReferenceException("Have you initialized the WpfModule because the customized InjectorFactory is missing.");

            var injector = injectorFactory.GetViewActionInjector(method);

            return new SubscriptionDirective(channel, injector, DeliveryThread.UserInterface);
        }
        #endregion
    }

    namespace ExtensionMethods
    {
        /// <summary>
        /// Extension methods for naming the message channels.
        /// </summary>
        public static class IWpfActionableExtensions
        {
            /// <summary>
            /// Channels the name for an event.
            /// </summary>
            /// <param name="attribute">The attribute.</param>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public static string ChannelNameFor(this EventInfo attribute, Type type)
            {
                return string.Format("action://{0}/{1}", type.Name, attribute.Name);
            }

            /// <summary>
            /// Channels the name for an attribute.
            /// </summary>
            /// <param name="attribute">The attribute.</param>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            public static string ChannelNameFor(this IViewActionable attribute, Type type)
            {
                return string.Format("action://{0}/{1}", type.Name, attribute.Action);
            }

            /// <summary>
            /// Channels the name.
            /// </summary>
            /// <param name="attribute">The attribute.</param>
            /// <returns></returns>
            public static string ChannelName(this SubscribeToActionAttribute attribute)
            {
                return string.Format("action://{0}/{1}", attribute.View, attribute.Action);
            }
        }
    }
}
