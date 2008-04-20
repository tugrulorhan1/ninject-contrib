using Ninject.Core;

namespace LoggingDemo.UI
{
    public class LoggingModule : AutoModule
    {
        public LoggingModule() : base(typeof(LoggingModule).Assembly)
        {
        }

        
    }
}