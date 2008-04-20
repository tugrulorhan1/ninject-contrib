using System.Windows;
using Ninject.Core;
using Ninject.Core.Logging;

namespace Ninject.Framework.PresentationFoundation
{
    /// <summary>
    /// A <see cref="Application"/> that creates a <see cref="IKernel"/> for use throughout
    /// the application.
    /// </summary>
    public abstract class NinjectWpfApplication : Application
    {

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [Inject]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.InitializeApplicationWith(CreateKernel());
        }

        /// <summary>
        /// Creates the kernel.
        /// </summary>
        /// <returns></returns>
        protected abstract IKernel CreateKernel();
    }
}
