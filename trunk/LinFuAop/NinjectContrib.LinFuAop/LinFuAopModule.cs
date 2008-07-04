using Ninject.Core;
using Ninject.Core.Interception;
using NinjectContrib.LinFuAop.Infrastructure;

namespace NinjectContrib.LinFuAop
{
    public class LinFuAopModule : StandardModule
    {
        public override void Load()
        {
            // We will use a method replacement strategy that checks
            // first whether the method should be replaced
            // The other option is to use SimpleMethodReplacementProviderFactory
            // which will always try to replace the method
            Kernel.Components.Connect<IMethodReplacementProviderFactory>(
                new CheckingMethodReplacementProviderFactory());

            Kernel.Components.Connect<IProxyFactory>(new LinFuAopProxyFactory());
        }
    }
}
