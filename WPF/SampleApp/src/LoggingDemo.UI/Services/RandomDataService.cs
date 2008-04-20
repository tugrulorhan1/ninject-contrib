using Ninject.Core;

namespace LoggingDemo.UI.Services
{
    [Service(typeof(IRandomDataService))]
    public class RandomDataService : IRandomDataService
    {
        private readonly IBlogService _blogService;
        private readonly IPostService _postService;

        [Inject]
        public RandomDataService(IBlogService blogService, IPostService postService)
        {
            _blogService = blogService;
            _postService = postService;
        }

        #region IRandomDataService Members

        public void AddRandomData(int numberOfBlogs)
        {
            for (int i = 0; i < numberOfBlogs; i++)
            {
                var blog = _blogService.InitializeWithRandomData();

                for (int j = 0; j < numberOfBlogs; j++)
                {
                    var post = _postService.InitializeWithRandomData();
                    post.Blog = blog;

                    _postService.Save(post);
                }
            }
        }

        #endregion
    }
}