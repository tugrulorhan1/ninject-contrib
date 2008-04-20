using System;
using System.Reflection;
using Ninject.Core.Infrastructure;
using Ninject.Core.Injection;

namespace Ninject.Framework.PresentationFoundation.Infrastructure
{
    /// <summary>
    /// A method injector that uses a dynamically-generated <see cref="Invoker"/> for invocation.
    /// </summary>
    [Serializable]
    public class DynamicViewActionInjector : InjectorBase<MethodInfo>, IMethodInjector
    {

        #region Fields
        [NonSerialized]
        private Invoker _invoker;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new DynamicViewActionInjector.
        /// </summary>
        /// <param name="member">The method that will be injected.</param>
        public DynamicViewActionInjector(MethodInfo member)
            : base(member)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Calls the method associated with the injector.
        /// </summary>
        /// <param name="target">The instance on which to call the method.</param>
        /// <param name="arguments">The arguments to pass to the method.</param>
        /// <returns>The return value of the method.</returns>
        public object Invoke(object target, params object[] arguments)
        {
            if (_invoker == null)
                _invoker = DynamicMethodFactory.CreateInvoker(Member);

            return Member.GetParameters().Length == arguments.Length 
                ? _invoker.Invoke(target, arguments) 
                : _invoker.Invoke(target, new object[0]);
        }
        #endregion
    }
}
