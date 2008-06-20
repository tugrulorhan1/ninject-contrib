using System;
using System.Windows;
using Ninject.Core;
using Ninject.Core.Infrastructure;
using Ninject.Framework.PresentationFoundation.Infrastructure;

namespace Ninject.Framework.PresentationFoundation
{
    /// <summary>
    /// A static container for the <see cref="NinjectWpfApplication"/>'s kernel.
    /// </summary>
    public static class KernelContainer
    {
        private static IKernel _kernel;

        /// <summary>
        /// Gets or sets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public static IKernel Kernel
        {
            get { return _kernel; }
            set
            {
                if (_kernel != null)
                    throw new NotSupportedException("The static container already has a kernel associated with it!");

                _kernel = value;
            }
        }

        /// <summary>
        /// Injects the specified instance by using the container's kernel.
        /// </summary>
        /// <param name="instance">The instance to inject.</param>
        public static void Inject(object instance)
        {
            if (_kernel == null)
            {
                throw new InvalidOperationException(String.Format(
                    "The type {0} requested an injection, but no kernel has been registered for the wpf application.\r\n" +
                    "Please ensure that your project defines a NinjectWpfApplication.",
                    instance.GetType()));
            }

            _kernel.Inject(instance);
        }



        /// <summary>
        /// Initializes the presenter for the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        public static void WireUp(this IView view)
        {
            Inject(view);

            var presentedBy = view.GetType().GetOneAttribute<PresentedByAttribute>();

            if (presentedBy == null) throw new InvalidOperationException("Did you specify a PresentedBy attribute on this view?");
            var presenterType = presentedBy.PresenterType;

            var presenter = (IPresenter)_kernel.Get(presenterType);
            if (presenter == null) throw new InvalidOperationException("Does your presenter subclass Presenterbase, it seems to have gotten lost.");

            presenter.SetView(view);
        }

        /// <summary>
        /// Initializes the application with a standard kernel for the specified modules.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="modules">The modules.</param>
        public static void InitializeApplicationWith(this Application application, params IModule[] modules)
        {
            application.InitializeApplicationWith(new StandardKernel(modules));
        }

        /// <summary>
        /// Initializes the application with the given kernel.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="kernel">The kernel.</param>
        public static void InitializeApplicationWith(this Application application, IKernel kernel)
        {
            Kernel = kernel;
            Inject(application);
        }
        
    }
}
