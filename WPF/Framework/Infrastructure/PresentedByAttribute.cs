using System;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// This attribute marks a view with it's acompanying presenter
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PresentedByAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the presenter.
        /// </summary>
        /// <value>The type of the presenter.</value>
        public Type PresenterType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentedByAttribute"/> class.
        /// </summary>
        /// <param name="presenterType">Type of the presenter.</param>
        public PresentedByAttribute(Type presenterType)
        {
            PresenterType = presenterType;
        }
    }
}
