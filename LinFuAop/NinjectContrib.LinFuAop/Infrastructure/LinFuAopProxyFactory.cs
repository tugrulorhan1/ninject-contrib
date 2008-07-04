using LinFu.AOP.Interfaces;
using Ninject.Core.Activation;
using Ninject.Core.Interception;

namespace NinjectContrib.LinFuAop.Infrastructure
{
    public class LinFuAopProxyFactory : ProxyFactoryBase
    {
        public override object Wrap(IContext context, object instance)
        {
            var modified = instance as IModifiableType;
            if (modified != null)
            {
                modified.IsInterceptionEnabled = true;
                modified.MethodReplacementProvider =
                    Kernel.Components.Get<IMethodReplacementProviderFactory>()
                        .Create(Kernel, context, instance);
            }
            // TODO: add an else check in here and wrap in a DynamicProxy...
            return instance;
        }

        public override object Unwrap(IContext context, object instance)
        {
            var modified = instance as IModifiableType;
            if (modified != null)
            {
                modified.IsInterceptionEnabled = false;
                modified.MethodReplacementProvider = null;
            }
            return instance;
        }
    }
}
