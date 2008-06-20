using Ninject.Core;
using Ninject.Core.Injection;
using Ninject.Extensions.MessageBroker.Infrastructure;
using Ninject.Framework.PresentationFoundation.Infrastructure;

namespace Ninject.Framework.PresentationFoundation
{
    /// <summary>
    /// Adds functionality to the kernel to support wpf.
    /// </summary>
    public class WpfModule : StandardModule
    {
        
        /// <summary>
        /// Prepares the module for being loaded. Can be used to connect component dependencies.
        /// </summary>
        public override void BeforeLoad()
        {
            Kernel.Components.Connect<IMessageBroker>(new WpfMessageBroker());
            Kernel.Components.Connect<IInjectorFactory>(new WpfDynamicInjectorFactory());
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            
        }
    }
}