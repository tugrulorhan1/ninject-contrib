using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggingDemo.UI;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Services;
using Ninject.Core;

namespace LoggingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //                _mockRepository.Expect(r => r.Find<Blog>()).Returns(_expectedBlogs).Verifiable();
            using (var kernel = new StandardKernel(new LoggingModule()))
            {
                var _blogService = kernel.Get<IBlogService>();
                //until here everything looks fine. but when i go into the service class 
                //the local variables are set to null
                var blogs = _blogService.FindAll();


                //                _mockRepository.VerifyAll();

            }

            Console.ReadKey();
        }
    }
}
