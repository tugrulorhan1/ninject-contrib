using LinFu.AOP.Interfaces;
using Ninject.Conditions;
using Ninject.Core;
using NinjectContrib.LinFuAop.Tests.Mocks;
using NinjectContrib.LinFuAop.Tests.PostwovenAssembly;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectContrib.LinFuAop.Tests
{
    [TestFixture]
    public class InterceptFixture
    {
        [Test]
        public void DynamicIntercepted()
        {
            var mockModule = new InlineModule(
                m => m.Bind<MockObject>().ToSelf().WithArguments(new { myProperty = "property", myField = "field" })
            );

            using (var kernel = new StandardKernel(new LinFuAopModule(), mockModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj, Is.InstanceOfType(typeof(IModifiableType)));
                Assert.That(((IModifiableType)obj).IsInterceptionEnabled, Is.False);
                Assert.That(((IModifiableType)obj).MethodReplacementProvider, Is.Null);

                Assert.That(obj.GetMyProperty(), Is.EqualTo("property"));
                Assert.That(obj.GetMyProperty2(), Is.EqualTo("property"));
                Assert.That(obj.GetMyField(), Is.EqualTo("field"));
                Assert.That(obj.GetMyField2(), Is.EqualTo("field"));
            }

            var testModule = new InlineModule(
                m => m.Intercept<StringReturnInterceptor>(When.Request.Method.Name.EqualTo("GetMyProperty")),
                m => m.Intercept<StringReturnInterceptor>(When.Request.Method.Name.EqualTo("GetMyField"))
            );

            using (var kernel = new StandardKernel(new LinFuAopModule(), mockModule, testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj, Is.InstanceOfType(typeof(IModifiableType)));
                Assert.That(((IModifiableType)obj).IsInterceptionEnabled, Is.True);
                Assert.That(((IModifiableType)obj).MethodReplacementProvider, Is.Not.Null);
                
                Assert.That(obj.GetMyProperty(), Is.EqualTo("intercepted"));
                Assert.That(obj.GetMyProperty2(), Is.EqualTo("property"));
                Assert.That(obj.GetMyField(), Is.EqualTo("intercepted"));
                Assert.That(obj.GetMyField2(), Is.EqualTo("field"));
            }
        }
    }
}
