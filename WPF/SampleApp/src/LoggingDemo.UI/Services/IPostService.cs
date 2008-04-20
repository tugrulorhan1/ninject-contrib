using LoggingDemo.UI.Model;

namespace LoggingDemo.UI.Services
{
    public interface IPostService : IDataService<Post>
    {
        void Save(Post post);
        Post InitializeWithRandomData();
    }
}