using System;
using LoggingDemo.UI.Interceptors;
using LoggingDemo.UI.Model;
using Ninject.Core;

namespace LoggingDemo.UI.Services
{
    [Service(typeof(IBlogService))]
    [LogMyCalls]
    public class BlogService : DataServiceBase<Blog>, IBlogService
    {
        [Inject]
        public BlogService(IRepository repository) : base(repository)
        {
        }

        #region IBlogService Members

        public virtual Blog Add(string name)
        {
            var blog = new Blog{ Name = name};

            _repository.Add(blog);
            
            _repository.CompleteUnitOfWork();

            return blog;
        }

        public Blog InitializeWithRandomData()
        {
            return new Blog {Name = Guid.NewGuid().ToString()};
        }

        #endregion
    }
}