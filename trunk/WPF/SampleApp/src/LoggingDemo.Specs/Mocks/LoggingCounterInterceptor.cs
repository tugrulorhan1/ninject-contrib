using LoggingDemo.UI.Interceptors;
using Ninject.Core.Logging;

namespace LoggingDemo.Specs.Mocks
{
    public class LoggingCounterInterceptor : LoggingInterceptor
    {
        public int Count { get; private set; }

        public int ErrorCount { get; private set; }

        public void Reset()
        {
            Count = ErrorCount = 0;
        }

        public LoggingCounterInterceptor(ILogger logger) : base(logger)
        {
        }

        protected override void BeforeInvoke(Ninject.Core.Interception.IInvocation invocation)
        {
            Count++;
            base.BeforeInvoke(invocation);
        }

        protected override void OnError(Ninject.Core.Interception.IInvocation invocation, System.Exception exception)
        {
            ErrorCount++;
            base.OnError(invocation, exception);
        }
    }
}