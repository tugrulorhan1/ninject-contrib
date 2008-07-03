using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NinjectContrib.Interception.Tests.Mocks
{
    public class MockObject
    {
        public virtual string MyProperty { get; set; }

        public MockObject(string myProperty)
        {
            MyProperty = myProperty;
        }

        public virtual string GetMyProperty()
        {
            return MyProperty;
        }

        public virtual void SetMyProperty(string myProperty)
        {
            MyProperty = myProperty;
        }
    }
}
