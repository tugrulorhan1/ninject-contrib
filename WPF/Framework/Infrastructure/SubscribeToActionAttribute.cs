using System;
using Ninject.Core.Infrastructure;
using Ninject.Extensions.MessageBroker;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    
    /// <summary>
    /// Indicates that the decorated method should receive events published to a message broker
    /// channel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class SubscribeToActionAttribute : Attribute, IViewActionable
    {

        /// <summary>
        /// Gets the name of the channel to subscribe to.
        /// </summary>
        public string Action { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeAttribute"/> class.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="action">The action the view broadcasts.</param>
        public SubscribeToActionAttribute(Type viewType, string action) : this(viewType.Name, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeAttribute"/> class.
        /// </summary>
        /// <param name="view">Type of the view.</param>
        /// <param name="action">The action the view broadcasts.</param>
        public SubscribeToActionAttribute(string view, string action)
        {
            Ensure.ArgumentNotNullOrEmptyString(action, "action");
            Ensure.ArgumentNotNull(view, "view");
            Action = action;
            View = view;
        }

        /// <summary>
        /// Gets or sets the name of the view for this presenter action.
        /// </summary>
        /// <value>The view.</value>
        public string View { get; private set; }
    }
}