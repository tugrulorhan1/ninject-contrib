namespace LoggingDemo.Specs.Mocks
{
    public interface IInterceptedServiceMock
    {
        void MethodWithoutBody();
        void MethodThatThrowsAnException();
    }
}