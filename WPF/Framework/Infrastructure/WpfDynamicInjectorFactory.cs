using System.Collections.Generic;
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;
using NinjectDynamicInjectorFactory = Ninject.Core.Injection.DynamicInjectorFactory;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// A helper class that uses lightweight code generation to create dynamic methods.
    /// </summary>
    public class WpfDynamicInjectorFactory : NinjectDynamicInjectorFactory
    {
        private readonly Dictionary<MethodInfo, IMethodInjector> _viewActionInjectors = new Dictionary<MethodInfo, IMethodInjector>();
        /// <summary>
        /// Creates a new View action injector.
        /// </summary>
        /// <param name="viewAction">The method that the injector will invoke.</param>
        /// <returns>A new injector for the View action.</returns>
        public IMethodInjector CreateViewActionInjector(MethodInfo viewAction)
        {
            return new DynamicViewActionInjector(viewAction);
        }

        /// <summary>
        /// Gets an injector for the specified View action.
        /// </summary>
        /// <param name="viewAction">The method that the injector will invoke.</param>
        /// <returns>A new injector for the View action.</returns>
        public IMethodInjector GetViewActionInjector(MethodInfo viewAction)
        {
            Guard.Against(viewAction.IsGenericMethodDefinition, "Cannot create injector from generic type definition.");

            if (_viewActionInjectors.ContainsKey(viewAction))
                return _viewActionInjectors[viewAction];

            var injector = CreateViewActionInjector(viewAction);
            _viewActionInjectors.Add(viewAction, injector);

            return injector;
        }
    }
}
