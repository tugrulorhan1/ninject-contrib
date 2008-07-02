using System.Collections.Generic;
using System.Reflection;
using Ninject.Core;

namespace NinjectContrib.Interception.Registry
{
    public class MethodInterceptorCollection : Dictionary<MethodInfo, List<IInterceptor>>
    {
        public void Add(MethodInfo method, IInterceptor interceptor)
        {
            if (!ContainsKey(method))
            {
                Add(method, new List<IInterceptor>());
            }
            this[method].Add(interceptor);
        }
    }
}
