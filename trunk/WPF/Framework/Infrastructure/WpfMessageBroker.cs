using System;
using Ninject.Core.Activation;
using Ninject.Core.Planning;
using Ninject.Extensions.MessageBroker.Infrastructure;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// Slightly customized implementation of a message broker.
    /// This one has support for our own publish/subscribe attributes
    /// </summary>
    public class WpfMessageBroker : StandardMessageBroker
    {
        #region Event Sources
        /// <summary>
        /// Called when the component is connected to its environment.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnConnected(EventArgs args)
        {
            base.OnConnected(args);

            var planner = Kernel.GetComponent<IPlanner>();
            planner.Strategies.Append(new WpfEventReflectionStrategy());

            var activator = Kernel.GetComponent<IActivator>();
            activator.Strategies.Append(new EventBindingStrategy());
        }

        /// <summary>
        /// Called when the component is disconnected from its environment.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected override void OnDisconnected(EventArgs args)
        {
            var planner = Kernel.GetComponent<IPlanner>();
            planner.Strategies.RemoveAll<WpfEventReflectionStrategy>();

            var activator = Kernel.GetComponent<IActivator>();
            activator.Strategies.RemoveAll<EventBindingStrategy>();

            base.OnDisconnected(args);
        }
        #endregion
    }
}
