using Ninject.Core;
using Ninject.Core.Interception;

namespace NinjectContrib.LinFuAop.Tests.Mocks
{
    public class StringReturnInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = "intercepted";
        }
    }
}
