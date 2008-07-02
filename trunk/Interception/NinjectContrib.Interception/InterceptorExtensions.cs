using System;
using System.Linq.Expressions;
using System.Reflection;
using Ninject.Core;
using Ninject.Core.Interception;
using NinjectContrib.Interception.Registry;

namespace NinjectContrib.Interception
{
    public static class InterceptorExtensions
    {
        public static void InterceptReplace<T>(this IModule module, Expression<Action<T>> methodExpr, Action<IInvocation> action)
        {
            module.AddMethodInterceptor(GetMethodFromExpression(methodExpr), action);
        }

        public static void InterceptAround<T>(this IModule module, Expression<Action<T>> methodExpr,
            Action<IInvocation> beforeAction, Action<IInvocation> afterAction)
        {
            module.InterceptReplace(methodExpr,
                i => { beforeAction(i); i.Proceed(); afterAction(i); });
        }

        public static void InterceptBefore<T>(this IModule module, Expression<Action<T>> methodExpr, Action<IInvocation> action)
        {
            module.InterceptAround(methodExpr, action, i => { });
        }

        public static void InterceptAfter<T>(this IModule module, Expression<Action<T>> methodExpr, Action<IInvocation> action)
        {
            module.InterceptAround(methodExpr, i => { }, action);
        }

        public static void InterceptReplaceGet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.AddMethodInterceptor(GetGetterFromExpression(propertyExpr), action);
        }

        public static void InterceptAroundGet<T>(this IModule module, Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> beforeAction, Action<IInvocation> afterAction)
        {
            module.InterceptReplaceGet(propertyExpr,
                i => { beforeAction(i); i.Proceed(); afterAction(i); });
        }

        public static void InterceptBeforeGet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.InterceptAroundGet(propertyExpr, action, i => { });
        }

        public static void InterceptAfterGet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.InterceptAroundGet(propertyExpr, i => { }, action);
        }

        public static void InterceptReplaceSet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.AddMethodInterceptor(GetSetterFromExpression(propertyExpr), action);
        }

        public static void InterceptAroundSet<T>(this IModule module, Expression<Func<T, object>> propertyExpr,
            Action<IInvocation> beforeAction, Action<IInvocation> afterAction)
        {
            module.InterceptReplaceSet(propertyExpr,
                i => { beforeAction(i); i.Proceed(); afterAction(i); });
        }

        public static void InterceptBeforeSet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.InterceptAroundSet(propertyExpr, action, i => { });
        }

        public static void InterceptAfterSet<T>(this IModule module, Expression<Func<T, object>> propertyExpr, Action<IInvocation> action)
        {
            module.InterceptAroundSet(propertyExpr, i => { }, action);
        }

        public static void AddMethodInterceptor(this IModule module, MethodInfo method, Action<IInvocation> action)
        {
            var interceptor = new ActionInterceptor(action);
            module.Kernel.Components.Get<IMethodInterceptorRegistry>().Add(method, interceptor);
        }

        private static MethodInfo GetMethodFromExpression<T>(Expression<Action<T>> methodExpr)
        {
            var call = methodExpr.Body as MethodCallExpression;
            if (call == null)
            {
                throw new InvalidOperationException("Expression must be a method call");
            }
            if (call.Object != methodExpr.Parameters[0])
            {
                throw new InvalidOperationException("Method call must target lambda argument");
            }
            return call.Method;
        }

        private static MethodInfo GetGetterFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            var propertyInfo = GetPropertyFromExpression(propertyExpr);
            if (!propertyInfo.CanRead)
            {
                throw new InvalidOperationException("Property must be readable");
            }
            return propertyInfo.GetGetMethod();
        }

        private static MethodInfo GetSetterFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            var propertyInfo = GetPropertyFromExpression(propertyExpr);
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException("Property must be writable");
            }
            return propertyInfo.GetSetMethod();
        }

        private static PropertyInfo GetPropertyFromExpression<T>(Expression<Func<T, object>> propertyExpr)
        {
            var body = propertyExpr.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }
            var memberExpr = body as MemberExpression;
            if (memberExpr == null || !(memberExpr.Member is PropertyInfo))
            {
                throw new InvalidOperationException("Expression must be a property access");
            }
            if (memberExpr.Expression != propertyExpr.Parameters[0])
            {
                throw new InvalidOperationException("Method call must target lambda argument");
            }
            return (PropertyInfo)memberExpr.Member;
        }
    }
}
