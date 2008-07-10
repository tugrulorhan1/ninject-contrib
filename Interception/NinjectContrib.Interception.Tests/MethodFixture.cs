using Ninject.Core;
using Ninject.Integration.LinFu;
using NinjectContrib.Interception.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectContrib.Interception.Tests
{
    [TestFixture]
    public class MethodFixture
    {
        [Test]
        public void MethodInterceptedWithReplace()
        {
            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule()))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(obj.GetMyProperty(), Is.EqualTo("start"));
            }

            var testModule = new InlineModule(
                m => m.InterceptReplace<MockObject>(o => o.GetMyProperty(),
                    i => i.ReturnValue = "intercepted")
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();
                
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(obj.GetMyProperty(), Is.EqualTo("intercepted"));
            }
        }

        [Test]
        public void MethodInterceptedBefore()
        {
            var testString = "empty";
            
            var testModule = new InlineModule(
                m => m.InterceptBefore<MockObject>(o => o.SetMyProperty(""),
                    i => testString = ((MockObject)i.Request.Target).MyProperty)
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();
                
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(testString, Is.EqualTo("empty"));
                
                obj.SetMyProperty("end");
                
                Assert.That(obj.MyProperty, Is.EqualTo("end"));
                Assert.That(testString, Is.EqualTo("start"));
            }
        }

        [Test]
        public void MethodInterceptedAfter()
        {
            var testString = "empty";
            
            var testModule = new InlineModule(
                m => m.InterceptAfter<MockObject>(o => o.SetMyProperty(""),
                    i => testString = ((MockObject)i.Request.Target).MyProperty)
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(testString, Is.EqualTo("empty"));

                obj.SetMyProperty("end");

                Assert.That(obj.MyProperty, Is.EqualTo("end"));
                Assert.That(testString, Is.EqualTo("end"));
            }
        }
    }
}
