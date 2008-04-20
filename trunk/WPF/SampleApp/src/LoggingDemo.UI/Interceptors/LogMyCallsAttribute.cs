using Ninject.Core;

namespace LoggingDemo.UI.Interceptors
{
    public class LogMyCallsAttribute : InterceptAttribute
    {
        public LogMyCallsAttribute() : base(typeof(LoggingInterceptor))
        {
        }
    }
}