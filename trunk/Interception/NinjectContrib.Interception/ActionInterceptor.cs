using System;
using Ninject.Core;
using Ninject.Core.Interception;

namespace NinjectContrib.Interception
{
    public class ActionInterceptor : IInterceptor
    {
        private readonly Action<IInvocation> interceptAction;

        public ActionInterceptor(Action<IInvocation> interceptAction)
        {
            this.interceptAction = interceptAction;
        }

        public void Intercept(IInvocation invocation)
        {
            interceptAction(invocation);
        }
    }
}
