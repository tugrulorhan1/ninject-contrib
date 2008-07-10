using System;
using Microsoft.Practices.Composite;
using Ninject.Core;

namespace NinjectContrib.CompositeWpf
{
    /// <summary>
    /// Defines a <seealso cref="IKernel"/> adapter for
    /// the <see cref="IContainerFacade"/> interface
    /// to be used by the Composite Application Library.
    /// </summary>
    public class NinjectContainerAdapter : IContainerFacade
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of <see cref="NinjectContainerAdapter"/>.
        /// </summary>
        /// <param name="kernel">The <seealso cref="IKernel"/> that will be used
        /// by the <see cref="Resolve"/> and <see cref="TryResolve"/> methods.</param>
        public NinjectContainerAdapter(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Resolve an instance of the requested type from the container.
        /// </summary>
        /// <param name="type">The type of object to get from the container.</param>
        /// <returns>An instance of <paramref name="type"/>.</returns>
        /// <exception cref="ActivationException"><paramref name="type"/> cannot be resolved by the container.</exception>
        public object Resolve(Type type)
        {
            return _kernel.Get(type);
        }

        /// <summary>
        /// Tries to resolve an instance of the requested type from the container.
        /// </summary>
        /// <param name="type">The type of object to get from the container.</param>
        /// <returns>
        /// An instance of <paramref name="type"/>. 
        /// If the type cannot be resolved it will return a <see langword="null"/> value.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public object TryResolve(Type type)
        {
            object resolved;

            try
            {
                resolved = Resolve(type);
            }
            catch
            {
                resolved = null;
            }

            return resolved;
        }
    }
}