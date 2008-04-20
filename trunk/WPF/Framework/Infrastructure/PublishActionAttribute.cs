using System;
using Ninject.Framework.PresentationFoundation.Infrastructure;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// Indicates that the decorated event should be published into a message broker channel.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event, AllowMultiple = true, Inherited = true)]
    public sealed class PublishActionAttribute : Attribute, IViewActionable
    {

        /// <summary>
        /// Gets the name of the channel to publish to.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishActionAttribute"/> class.
        /// </summary>
        /// <param name="action">The name of the channel to publish to.</param>
        public PublishActionAttribute(string action)
        {
            Action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishActionAttribute"/> class.
        /// </summary>
        public PublishActionAttribute()
        {
            Action = string.Empty;
        }
    }
}