using LinFu.AOP.Interfaces;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    public class CheckingMethodReplacementProvider : BaseMethodReplacementProvider
    {
        public IKernel Kernel { get; set; }
        public IContext Context { get; set; }
        public object Instance { get; set; }

        public CheckingMethodReplacementProvider(IKernel kernel, IContext context, object instance)
        {
            Kernel = kernel;
            Context = context;
            Instance = instance;
        }

        protected override bool ShouldReplace(IInvocationContext context)
        {
            IRequest request = Kernel.Components.Get<IRequestFactory>().Create(Context, Instance,
                context.TargetMethod, context.Arguments, context.TypeArguments);

            var interceptorRegistry = Kernel.Components.Get<IInterceptorRegistry>();

            // Would be nicer to have an IInterceptorRegistry.HasInterceptors(IRequest)
            return interceptorRegistry.GetInterceptors(request).Count > 0;
        }

        protected override IMethodReplacement GetReplacement(IInvocationContext context)
        {
            return new LinFuAopWrapper(Kernel, Context, Instance);
        }
    }
}
