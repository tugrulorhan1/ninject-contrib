using LinFu.AOP.Interfaces;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    // Creates a MethodReplacementProvider that checks whether it should
    // replace the method or not based on the Ninject context/request
    public class CheckingMethodReplacementProviderFactory : KernelComponentBase, IMethodReplacementProviderFactory
    {
        public IMethodReplacementProvider Create(IKernel kernel, IContext context, object instance)
        {
            return new CheckingMethodReplacementProvider(kernel, context, instance);
        }
    }
}
