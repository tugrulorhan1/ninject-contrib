using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Infrastructure;

namespace NinjectContrib.Interception.Registry
{
    public class StandardMethodInterceptorRegistry : KernelComponentBase, IMethodInterceptorRegistry
    {
        protected Dictionary<Type, MethodInterceptorCollection> typeMethods =
            new Dictionary<Type, MethodInterceptorCollection>();

        public void Add(MethodInfo method, IInterceptor interceptor)
        {
            var type = method.DeclaringType;
            if (!typeMethods.ContainsKey(type))
            {
                typeMethods.Add(type, new MethodInterceptorCollection());
            }
            typeMethods[type].Add(method, interceptor);
        }

        public bool Contains(Type type)
        {
            return typeMethods.ContainsKey(type);
        }

        public MethodInterceptorCollection GetMethodInterceptors(Type type)
        {
            return typeMethods[type];
        }
    }
}
