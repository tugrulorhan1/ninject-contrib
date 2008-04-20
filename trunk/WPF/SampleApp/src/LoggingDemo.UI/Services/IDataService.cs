using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;

namespace LoggingDemo.UI.Services
{
    public interface IDataService<TEntity> where TEntity : EntityBase
    {
        ObservableCollection<TEntity> FindAll();
        TEntity FindOne(int id);
    }
}