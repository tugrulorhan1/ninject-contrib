using LinFu.AOP.Interfaces;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    public class LinFuAopWrapper : StandardWrapper, IMethodReplacement
    {
        public LinFuAopWrapper(IKernel kernel, IContext context, object instance)
            : base(kernel, context, instance)
        {
        }

        public object Invoke(IInvocationContext context)
        {
            IRequest request = Kernel.Components.Get<IRequestFactory>().Create(Context, Instance,
                context.TargetMethod, context.Arguments, context.TypeArguments);

            IInvocation invocation = CreateInvocation(request);

            invocation.Proceed();

            return invocation.ReturnValue;
        }
    }
}
