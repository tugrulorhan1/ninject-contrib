using System;

namespace LoggingDemo.Specs.Mocks
{
    [LogMyCallsCounter]
    public class InterceptedServiceMock : IInterceptedServiceMock
    {
        public virtual void MethodWithoutBody()
        {
            // Nothing to do here
        }

        public virtual void MethodThatThrowsAnException()
        {
            throw new Exception("Because I can.");
        }
    }
}