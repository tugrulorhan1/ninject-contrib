using System;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Infrastructure;

namespace NinjectContrib.Interception.Registry
{
    public interface IMethodInterceptorRegistry : IKernelComponent
    {
        void Add(MethodInfo method, IInterceptor interceptor);
        bool Contains(Type type);
        MethodInterceptorCollection GetMethodInterceptors(Type type);
    }
}
