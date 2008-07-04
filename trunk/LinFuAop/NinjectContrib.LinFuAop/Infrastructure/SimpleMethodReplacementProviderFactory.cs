using LinFu.AOP.Interfaces;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    // Uses LinFu's inbuilt SimpleMethodReplacementProvider and will always
    // try to replace the method
    public class SimpleMethodReplacementProviderFactory : KernelComponentBase, IMethodReplacementProviderFactory
    {
        public IMethodReplacementProvider Create(IKernel kernel, IContext context, object instance)
        {
            return new SimpleMethodReplacementProvider(
                new LinFuAopWrapper(Kernel, context, instance));
        }
    }
}
