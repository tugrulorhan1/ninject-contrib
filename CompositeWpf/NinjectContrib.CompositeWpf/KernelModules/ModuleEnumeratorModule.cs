using Microsoft.Practices.Composite.Modularity;
using Ninject.Core;

namespace NinjectContrib.CompositeWpf.KernelModules
{
    /// <summary>
    /// A simple module to bind the default <seealso cref="IModuleEnumerator"/>
    /// </summary>
    public class ModuleEnumeratorModule : StandardModule
    {
        private readonly IModuleEnumerator moduleEnumerator;

        /// <summary>
        /// Instantiates a new <seealso cref="ModuleEnumeratorModule"/>
        /// by passing in the default <seealso cref="IModuleEnumerator"/>
        /// </summary>
        /// <param name="moduleEnumerator">The default <seealso cref="IModuleEnumerator"/></param>
        public ModuleEnumeratorModule(IModuleEnumerator moduleEnumerator)
        {
            this.moduleEnumerator = moduleEnumerator;
        }

        /// <summary>
        /// Loads the module into the <seealso cref="IKernel"/>.
        /// </summary>
        public override void Load()
        {
            if (moduleEnumerator != null)
            {
                Bind<IModuleEnumerator>().ToConstant(moduleEnumerator);
            }
        }
    }
}
