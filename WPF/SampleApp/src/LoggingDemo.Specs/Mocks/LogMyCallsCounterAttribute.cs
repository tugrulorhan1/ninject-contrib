using System;
using LoggingDemo.UI.Interceptors;
using Ninject.Core;

namespace LoggingDemo.Specs.Mocks
{
    public class LogMyCallsCounterAttribute : InterceptAttribute
    {
        public LogMyCallsCounterAttribute() : base(typeof(LoggingCounterInterceptor))
        {
        }
    }
}