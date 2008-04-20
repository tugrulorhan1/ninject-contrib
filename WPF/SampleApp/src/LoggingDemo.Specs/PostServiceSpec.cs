using System;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Services;
using Moq;
using Ninject.Core;
using Ninject.Core.Logging;
using NSpecify.Framework;

namespace LoggingDemo.Specs
{
    [Context(Description =  "Describes the behavior of the PostService")]
    public class PostServiceSpec
    {
        private Mock<IRepository> _mockRepository;
        private IPostService _postService;

        [BeforeAll]
        public void Before()
        {
            _mockRepository = new Mock<IRepository>();

            _postService = new PostService(_mockRepository.Object);
        }

        [Specification("Should add a new item to the UoW before completing it")]
        public void ShouldAddNewItemToUoW()
        {
            var post = new Post
                           {Title = "Test title", BlogId = 1, Description = "Test description", PostDate = DateTime.Now};

            _mockRepository.Expect(r => r.Add(It.IsAny<Post>())).Verifiable();
            _mockRepository.Expect(r => r.CompleteUnitOfWork()).Verifiable();

            _postService.Save(post);

            _mockRepository.VerifyAll();
        }
    }
}