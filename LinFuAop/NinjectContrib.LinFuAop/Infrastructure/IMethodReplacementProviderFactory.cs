using LinFu.AOP.Interfaces;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Infrastructure;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    public interface IMethodReplacementProviderFactory : IKernelComponent
    {
        IMethodReplacementProvider Create(IKernel kernel, IContext context, object instance);
    }
}
