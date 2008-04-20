using System;
using Ninject.Core.Interception;
using Ninject.Core.Logging;

namespace LoggingDemo.UI.Interceptors
{
    public class LoggingInterceptor : SimpleFailureInterceptor
    {
        private readonly ILogger _logger;
        private bool _hasError;

        public LoggingInterceptor(ILogger logger)
        {
            _logger = logger;
            _hasError = false;
        }
        protected override void BeforeInvoke(IInvocation invocation)
        {
            _logger.Debug("About to invoke {0}", MethodNameFor(invocation));
        }

        protected override void OnError(IInvocation invocation, Exception exception)
        {
            _logger.Error(exception, "There was an error invoking {0}.\r\n", MethodNameFor(invocation));
            _hasError = true;
            base.OnError(invocation, exception);
        }

        protected override void AfterInvoke(IInvocation invocation)
        {
            _logger.Debug("invocation of {0} finished {1}.", MethodNameFor(invocation), (_hasError ? "with an error state" : "successfully"));
        }

        private static string MethodNameFor(IInvocation invocation)
        {
            return invocation.Request.Method.Name;
        }
    }
}