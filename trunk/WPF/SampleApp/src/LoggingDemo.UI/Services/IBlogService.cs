using LoggingDemo.UI.Model;

namespace LoggingDemo.UI.Services
{
    public interface IBlogService : IDataService<Blog>
    {
        Blog Add(string name);
        Blog InitializeWithRandomData();
    }
}