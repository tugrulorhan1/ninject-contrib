using System.Collections.Generic;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Services;
using Moq;
using Ninject.Core;
using Ninject.Integration.LinFu;
using Ninject.Integration.NLog;
using NSpecify.Framework;
using NSpecify.Framework.Extensions;

namespace LoggingDemo.Specs
{

    [Context(Description = "Specifies the behavior for the BlogService")]
    public class BlogServiceSpec
    {
        private IBlogService _blogService;
        private IList<Blog> _expectedBlogs;
        private Mock<IRepository> _mockRepository;
        private IKernel _kernel;

        [BeforeAll]
        public void Before()
        {
            _mockRepository = new Mock<IRepository>();
            var inlineModule = new InlineModule(m =>
                                                    {
                                                        m.Bind<IRepository>().ToConstant(_mockRepository.Object);
                                                        m.Bind<IBlogService>().To<BlogService>();
                                                    });

            _expectedBlogs = new List<Blog>
                                     {
                                         new Blog {Name = "Blog 1"},
                                         new Blog {Name = "Blog 2"}
                                     };

            _kernel = new StandardKernel(new LinFuModule(), new NLogModule(), inlineModule);
            _blogService = _kernel.Get<IBlogService>();
        }

        [Specification("Find All should return all the blogs")]
        public void ShouldFindAll()
        {

            _mockRepository.Expect(r => r.Find<Blog>()).Returns(_expectedBlogs).Verifiable();
            var blogs = _blogService.FindAll();
            _mockRepository.VerifyAll();
            blogs.Must<Blog>().Not.Be.Empty("There should be some elements in the blogs collection");
            blogs.Count.Must().Equal(_expectedBlogs.Count, "There number of elements should be equal");
            blogs.Must<Blog>().Be.EquivalentTo(_expectedBlogs, "The blogs collection should be the same");

        }

        [Specification("Should find the blog with the correct Id")]
        public void ShouldFindOneById()
        {
            var id = 5;
            var expectedBlog = _expectedBlogs[1];
            _mockRepository.Expect(r => r.FindOne<Blog>(It.Is<int>(p => p == id))).Returns(expectedBlog);

            var blog = _blogService.FindOne(id);

            _mockRepository.VerifyAll();
            
            blog.Must().Not.Be.Null("We were expecting a return value here");
            blog.Must().Equal(blog, "The retrieved blog should be equal to the expected blog.");
        }

        [Specification("Add should add a blog if the name doesn't exist")]
        public void Add_ShouldAddBlog_IfNameDoesntExist()
        {
            var name = "Blog 3";
            
            _mockRepository.Expect(r => r.Add(It.Is<Blog>(b => b.Name == name))).Verifiable();
            _mockRepository.Expect(r => r.CompleteUnitOfWork()).Verifiable();

            var actual = _blogService.Add(name);
            _mockRepository.VerifyAll();

            actual.Must().Not.Be.Null();
            actual.Name.Must().Equal(name);

        }

        [AfterAll]
        public void After()
        {
            _kernel.Dispose();
        }

    }



}
