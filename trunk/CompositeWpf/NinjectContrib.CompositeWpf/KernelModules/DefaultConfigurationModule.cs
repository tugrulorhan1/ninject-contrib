using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Creation;

namespace NinjectContrib.CompositeWpf.KernelModules
{
    /// <summary>
    /// This module binds all the default Composite WPF services
    /// </summary>
    public class DefaultConfigurationModule : StandardModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IContainerFacade>().To<NinjectContainerAdapter>().Using<SingletonBehavior>();
            
            Bind<IEventAggregator>().To<EventAggregator>().Using<SingletonBehavior>();
            
            Bind<RegionAdapterMappings>().ToSelf().Using<SingletonBehavior>();

            Bind<IRegionManager>().ToProvider<RegionManagerProvider>().Using<SingletonBehavior>();
            
            Bind<IModuleLoader>().To<ModuleLoader>().Using<SingletonBehavior>();
        }
    }

    /// <summary>
    /// This provider is to get around the problem that the default constructor
    /// of <seealso cref="RegionManager"/> will be used by Ninject if a basic binding is provided
    /// </summary>
    public class RegionManagerProvider : SimpleProvider<IRegionManager>
    {
        /// <summary>
        /// Creates a new instance of a <seealso cref="RegionManager"/>.
        /// </summary>
        /// <param name="context">The context in which the activation is occurring.</param>
        /// <returns>A new <seealso cref="RegionManager"/></returns>
        protected override IRegionManager CreateInstance(IContext context)
        {
            return new RegionManager(context.Kernel.Get<RegionAdapterMappings>());
        }
    }
}
