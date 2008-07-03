using Ninject.Core;
using Ninject.Integration.LinFu;
using NinjectContrib.Interception.Tests.Mocks;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NinjectContrib.Interception.Tests
{
    [TestFixture]
    public class PropertyFixture
    {
        [Test]
        public void PropertyGetInterceptedWithReplace()
        {
            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule()))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.GetMyProperty(), Is.EqualTo("start"));
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
            }

            var testModule = new InlineModule(
                m => m.InterceptReplaceGet<MockObject>(o => o.MyProperty, i => i.ReturnValue = "intercepted")
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();
                
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.GetMyProperty(), Is.EqualTo("start"));
                Assert.That(obj.MyProperty, Is.EqualTo("intercepted"));
            }
        }

        [Test]
        public void PropertySetInterceptedWithReplace()
        {
            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule()))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                
                obj.MyProperty = "end";

                Assert.That(obj.MyProperty, Is.EqualTo("end"));
            }

            var testModule = new InlineModule(
                m => m.InterceptReplaceSet<MockObject>(o => o.MyProperty, i => {})
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));

                obj.MyProperty = "end";

                Assert.That(obj.MyProperty, Is.EqualTo("start"));
            }
        }

        [Test]
        public void PropertyGetInterceptedBefore()
        {
            var testString = "empty";

            var testModule = new InlineModule(
                m => m.InterceptBeforeGet<MockObject>(o => o.MyProperty, i => { if (i.ReturnValue == null) { testString = "null"; } })
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();
                
                Assert.That(obj, Is.Not.Null);
                Assert.That(testString, Is.EqualTo("empty"));
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(testString, Is.EqualTo("null"));
            }
        }

        [Test]
        public void PropertySetInterceptedBefore()
        {
            var testModule = new InlineModule(
                m => m.InterceptBeforeSet<MockObject>(o => o.MyProperty, i => i.Request.Arguments[0] = "intercepted")
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                
                obj.MyProperty = "end";
                
                Assert.That(obj.MyProperty, Is.EqualTo("intercepted"));
            }
        }

        [Test]
        public void PropertyGetInterceptedAfter()
        {
            var testString = "empty";
            
            var testModule = new InlineModule(
                m => m.InterceptAfterGet<MockObject>(o => o.MyProperty, i => testString = i.ReturnValue.ToString())
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(testString, Is.EqualTo("empty"));
                Assert.That(obj.MyProperty, Is.EqualTo("start"));
                Assert.That(testString, Is.EqualTo("start"));
            }
        }

        [Test]
        public void PropertySetInterceptedAfter()
        {
            var testString = "empty";
            
            var testModule = new InlineModule(
                m => m.InterceptAfterSet<MockObject>(o => o.MyProperty, i => testString = ((MockObject)i.Request.Target).MyProperty)
            );

            using (var kernel = new StandardKernel(new LinFuModule(), new MethodInterceptorModule(), new MockModule(), testModule))
            {
                var obj = kernel.Get<MockObject>();

                Assert.That(obj, Is.Not.Null);
                Assert.That(testString, Is.EqualTo("empty"));
                Assert.That(obj.MyProperty, Is.EqualTo("start"));

                obj.MyProperty = "end";

                Assert.That(obj.MyProperty, Is.EqualTo("end"));
                Assert.That(testString, Is.EqualTo("end"));
            }
        }
    }
}
