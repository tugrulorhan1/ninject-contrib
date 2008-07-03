using Ninject.Core;

namespace NinjectContrib.Interception.Tests.Mocks
{
    class MockModule : StandardModule
    {
        public override void Load()
        {
            Bind<MockObject>().ToSelf().WithArgument("myProperty", "start");
        }
    }
}
