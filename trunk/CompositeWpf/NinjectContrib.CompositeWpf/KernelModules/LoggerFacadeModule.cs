using Microsoft.Practices.Composite.Logging;
using Ninject.Core;

namespace NinjectContrib.CompositeWpf.KernelModules
{
    /// <summary>
    /// A simple module to bind the default <seealso cref="ILoggerFacade"/>
    /// </summary>
    public class LoggerFacadeModule : StandardModule
    {
        private readonly ILoggerFacade loggerFacade;

        /// <summary>
        /// Instantiates a new <seealso cref="LoggerFacadeModule"/>
        /// by passing in the default <seealso cref="ILoggerFacade"/>
        /// </summary>
        /// <param name="loggerFacade">The default <seealso cref="ILoggerFacade"/></param>
        public LoggerFacadeModule(ILoggerFacade loggerFacade)
        {
            this.loggerFacade = loggerFacade;
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ILoggerFacade>().ToConstant(loggerFacade);
        }
    }
}
