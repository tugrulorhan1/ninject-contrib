using System;
using Ninject.Core.Binding;
using Ninject.Core.Infrastructure;
using Ninject.Core.Interception;
using Ninject.Core.Planning;
using Ninject.Core.Planning.Directives;
using Ninject.Core.Planning.Strategies;
using NinjectContrib.Interception.Registry;

namespace NinjectContrib.Interception.Strategy
{
    public class MethodInterceptorRegistrationStrategy : PlanningStrategyBase
    {
        public override StrategyResult Build(IBinding binding, Type type, IActivationPlan plan)
        {
            var methodInterceptorRegistry = Kernel.Components.Get<IMethodInterceptorRegistry>();
            if (methodInterceptorRegistry.Contains(type))
            {
                var methodInterceptors = methodInterceptorRegistry.GetMethodInterceptors(type);
                var interceptorRegistry = Kernel.Components.Get<IInterceptorRegistry>();

                foreach (var method in methodInterceptors.Keys)
                {
                    for (int order = 0; order < methodInterceptors[method].Count; order++)
                    {
                        var interceptor = methodInterceptors[method][order];
                        interceptorRegistry.RegisterStatic(r => interceptor, order, method);
                    }
                }

                if (!plan.Directives.HasOneOrMore<ProxyDirective>())
                {
                    plan.Directives.Add(new ProxyDirective());
                }
            }
            return StrategyResult.Proceed;
        }
    }
}
