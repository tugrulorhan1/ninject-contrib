using System.Collections.Generic;
using LoggingDemo.Specs.Mocks;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Interception;
using Ninject.Integration.LinFu;
using Ninject.Integration.NLog;
using NSpecify.Framework;

namespace LoggingDemo.Specs
{
    [Context(Description = "Specifies the behavior for the LogMyCallsInterceptor")]
    public class LogMyCallsInterceptorSpec
    {
        private IKernel _kernel;

        [BeforeEach]
        public void Before()
        {
            var inlineModule = new InlineModule(m => m.Bind<IInterceptedServiceMock>().To<InterceptedServiceMock>());

            _kernel = new StandardKernel(new LinFuModule(), new NLogModule(), inlineModule);
        }

        [AfterEach]
        public void After()
        {
            _kernel.Dispose();
        }

        [Specification("All this should do is show the calls in the test runner. It should log to the console")]
        public void ShouldShowCallsInConsole()
        {
            var service = _kernel.Get<IInterceptedServiceMock>();
            IContext context = new StandardContext(_kernel, typeof(IInterceptedServiceMock));

            IRequest request = new StandardRequest(
                context,
                service,
                typeof(InterceptedServiceMock).GetMethod("MethodWithoutBody"),
                new object[0]
            );

            var interceptors = _kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);

            var enumerator = interceptors.GetEnumerator();
            enumerator.MoveNext();

            Specify.That(interceptors.Count).Must.Equal(1, "There should be 1 interceptor registered");
            Specify.That(enumerator.Current).Must.Be.InstanceOf(typeof(LoggingCounterInterceptor));

            var interceptor = enumerator.Current as LoggingCounterInterceptor;

            service.MethodWithoutBody();

            Specify.That(interceptor).Must.Not.Be.Null();
            Specify.That(interceptor.Count).Must.Equal(1, "There should be 1 invocation counted.");
        }

        [Specification("Should show valid counts for a number of invocations")]
        public void Should_Show_Correct_Counts_For_Number_Of_Invocations()
        {
            var service = _kernel.Get<IInterceptedServiceMock>();
            IContext context = new StandardContext(_kernel, typeof(IInterceptedServiceMock));

            IRequest request = new StandardRequest(
                context,
                service,
                typeof(InterceptedServiceMock).GetMethod("MethodWithoutBody"),
                new object[0]
            );

            var interceptors = _kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);

            var enumerator = interceptors.GetEnumerator();
            enumerator.MoveNext();

            Specify.That(interceptors.Count).Must.Equal(1, "There should be 1 interceptor registered");
            Specify.That(enumerator.Current).Must.Be.InstanceOf(typeof(LoggingCounterInterceptor));

            var interceptor = enumerator.Current as LoggingCounterInterceptor;

            service.MethodWithoutBody();
            service.MethodWithoutBody();
            service.MethodWithoutBody();

            Specify.That(interceptor).Must.Not.Be.Null();
            Specify.That(interceptor.Count).Must.Equal(3, "There should be 3 invocations counted.");
            Specify.That(interceptor.ErrorCount).Must.Equal(0, "There should be no errors counted.");
        }

        [Specification("Should have the correct count of invocations and the correct error count.")]
        public void Should_Have_Correct_Invocation_And_Error_Count()
        {
            var service = _kernel.Get<IInterceptedServiceMock>();
            var context = new StandardContext(_kernel, typeof(IInterceptedServiceMock));

            var request = new StandardRequest(
                context,
                service,
                typeof(InterceptedServiceMock).GetMethod("MethodWithoutBody"),
                new object[0]
            );

            var errorRequest = new StandardRequest(
                context,
                service,
                typeof (InterceptedServiceMock).GetMethod("MethodThatThrowsAnException"),
                new object[0]
            );

            var interceptors = _kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(request);
            var errorInterceptors = _kernel.GetComponent<IInterceptorRegistry>().GetInterceptors(errorRequest);

            var enumerator = interceptors.GetEnumerator();
            enumerator.MoveNext();
            var errorEnumerator = errorInterceptors.GetEnumerator();
            errorEnumerator.MoveNext();

            Specify.That(interceptors.Count).Must.Equal(1, "There should be 1 interceptor registered");
            Specify.That(enumerator.Current).Must.Be.InstanceOf(typeof(LoggingCounterInterceptor));
            Specify.That(errorInterceptors.Count).Must.Equal(1, "There should be 1 error interceptor registered");
            Specify.That(errorEnumerator.Current).Must.Be.InstanceOf(typeof(LoggingCounterInterceptor));

            var interceptor = enumerator.Current as LoggingCounterInterceptor;
            var errorInterceptor = errorEnumerator.Current as LoggingCounterInterceptor;

            service.MethodWithoutBody();
            service.MethodWithoutBody();
            service.MethodWithoutBody();

            try
            {
                service.MethodThatThrowsAnException();
            }
            catch
            {
            }

            Specify.That(interceptor).Must.Not.Be.Null();
            Specify.That(interceptor.Count).Must.Equal(3, "There should be 3 invocations counted.");
            Specify.That(errorInterceptor.Count).Must.Equal(1, "There should be 1 invocation counted.");
            Specify.That(errorInterceptor.ErrorCount).Must.Equal(1, "There should be 1 error counted.");
        }
    }
}