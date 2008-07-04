namespace NinjectContrib.LinFuAop.Tests.PostwovenAssembly
{
    public class MockObject
    {
        private readonly string myField;
        private string myProperty { get; set; }

        public MockObject(string myProperty, string myField)
        {
            this.myProperty = myProperty;
            this.myField = myField;
        }

        public string GetMyProperty()
        {
            return myProperty;
        }

        public string GetMyProperty2()
        {
            return myProperty;
        }

        public string GetMyField()
        {
            return myField;
        }

        public string GetMyField2()
        {
            return myField;
        }
    }
}
