using Ninject.Core;
using Ninject.Core.Planning;
using NinjectContrib.Interception.Strategy;
using NinjectContrib.Interception.Registry;

namespace NinjectContrib.Interception
{
    public class MethodInterceptorModule : StandardModule
    {
        public override void Load()
        {
            Kernel.Components.Connect<IMethodInterceptorRegistry>(
                new StandardMethodInterceptorRegistry());

            Kernel.Components.Get<IPlanner>().Strategies.Append(
                new MethodInterceptorRegistrationStrategy());
        }
    }
}
