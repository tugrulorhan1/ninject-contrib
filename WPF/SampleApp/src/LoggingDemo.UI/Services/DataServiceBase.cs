using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;
using Ninject.Core;
using Ninject.Core.Logging;

namespace LoggingDemo.UI.Services
{
    public abstract class DataServiceBase<TEntity> : IDataService<TEntity> where TEntity : EntityBase
    {
        protected readonly IRepository _repository;
        protected ILogger _logger = NullLogger.Instance;

        [Inject]
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        [Inject]
        protected DataServiceBase(IRepository repository)
        {
            _repository = repository;
        }

        #region IDataService<TEntity> Members

        public virtual ObservableCollection<TEntity> FindAll()
        {
            return new ObservableCollection<TEntity>( _repository.Find<TEntity>() as List<TEntity>);
        }

        public virtual TEntity FindOne(int id)
        {
            return _repository.FindOne<TEntity>(id);
        }

        #endregion
    }
}