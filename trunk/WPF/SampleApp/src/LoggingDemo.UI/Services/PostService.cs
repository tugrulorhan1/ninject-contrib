using System;
using LoggingDemo.UI.Interceptors;
using LoggingDemo.UI.Model;
using Ninject.Core;

namespace LoggingDemo.UI.Services
{
    [Service(typeof(IPostService)), LogMyCalls]
    public class PostService : DataServiceBase<Post>, IPostService
    {
        [Inject]
        public PostService(IRepository repository) : base(repository)
        {
        }

        #region IPostService Members

        public virtual void Save(Post post)
        {
            if(post.IsNew)
                _repository.Add(post);

            _repository.CompleteUnitOfWork();
        }

        public Post InitializeWithRandomData()
        {
            return new Post{Title = Guid.NewGuid().ToString(), Description = "Post content goes here", PostDate = DateTime.Now.AddDays(-5)};
        }

        #endregion
    }
}