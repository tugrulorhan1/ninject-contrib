using System;
using System.Collections.Generic;
using System.Data;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Querying;
using Ninject.Core;

namespace LoggingDemo.UI.Model
{
    [Singleton, Service(typeof(IRepository))]
    public class DemoRepository : IRepository
    {
        #region IRepository Members

        /// <summary>
        /// Finds all the entities of the given type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public virtual IList<Entity> Find(Type entityType)
        {
            return Repository.Find(entityType);
        }

        /// <summary>
        /// Finds all the entities of the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public virtual IList<TEntity> Find<TEntity>() where TEntity : EntityBase
        {
            return Repository.Find<TEntity>();
        }

        /// <summary>
        /// Finds all the entities of the given type that satisfy the given conditions.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual IList<Entity> Find(Type entityType, QueryExpression expression)
        {
            return Repository.Find(entityType, expression);
        }

        /// <summary>
        /// Finds all the entities of the given type that satisfy the given conditions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The query expression.</param>
        /// <returns></returns>
        public virtual IList<TEntity> Find<TEntity>(QueryExpression expression) where TEntity : EntityBase
        {
            return Repository.Find<TEntity>(expression);
        }

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified conditions.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="order">The order.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public virtual IList<Entity> Find(Type entityType, QueryExpression expression, Order order, Page page)
        {
            return Repository.Find(entityType, expression, order, page);
        }

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified conditions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="order">The order.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public virtual IList<TEntity> Find<TEntity>(QueryExpression expression, Order order, Page page) where TEntity : EntityBase
        {
            return Repository.Find<TEntity>(expression, order, page);
        }

        /// <summary>
        /// Finds all the entities yielded by the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual IList<Entity> Find(Query query)
        {
            return Repository.Find(query);
        }

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual IList<TEntity> Find<TEntity>(Query query) where TEntity : EntityBase
        {
            return Repository.Find<TEntity>(query);
        }

        /// <summary>
        /// Finds one entity by Id.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual Entity FindOne(Type entityType, int id)
        {
            return Repository.FindOne(entityType, id);
        }

        /// <summary>
        /// Finds one entity by Id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual TEntity FindOne<TEntity>(int id) where TEntity : EntityBase
        {
            return Repository.FindOne<TEntity>(id);
        }

        /// <summary>
        /// Finds the first entity returned for the specified expression.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual Entity FindOne(Type entityType, QueryExpression expression)
        {
            return Repository.FindOne(entityType, expression);
        }

        /// <summary>
        /// Finds the first entity returned for the specified expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The query expression.</param>
        /// <returns></returns>
        public virtual TEntity FindOne<TEntity>(QueryExpression expression) where TEntity : EntityBase
        {
            return Repository.FindOne<TEntity>(expression);
        }

        /// <summary>
        /// Finds the first entity returned for the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual Entity FindOne(Query query)
        {
            return Repository.FindOne(query);
        }

        /// <summary>
        /// Finds the first entity returned for the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual TEntity FindOne<TEntity>(Query query) where TEntity : EntityBase
        {
            return Repository.FindOne<TEntity>(query);
        }

        /// <summary>
        /// Returns the number of entities in the database for the specified query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual long Count(Query query)
        {
            return Repository.Count(query);
        }

        /// <summary>
        /// Returns the number of all the entities in the database
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public virtual long Count<TEntity>() where TEntity : EntityBase
        {
            return Repository.Count<TEntity>();
        }

        /// <summary>
        /// Returns the number of entities in the database for the specified query
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual long Count<TEntity>(Query query) where TEntity : EntityBase
        {
            return Repository.Count<TEntity>(query);
        }

        /// <summary>
        /// Projects the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public virtual IDataReader Project(Query query)
        {
            return Repository.Project(query);
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entityToRemove">The entity to remove.</param>
        public virtual void Remove(EntityBase entityToRemove)
        {
            Repository.Remove(entityToRemove);
        }

        /// <summary>
        /// Saves the pending changes and flushes the unit of work
        /// </summary>
        public virtual void SaveChanges()
        {
            Repository.SaveChanges();
        }

        /// <summary>
        /// Saves the pending changes and optionally resets the unit of work.
        /// </summary>
        /// <param name="resetUnitOfWork">if set to <c>true</c> [reset unit of work].</param>
        public virtual void SaveChanges(bool resetUnitOfWork)
        {
            Repository.SaveChanges(resetUnitOfWork);
        }

        /// <summary>
        /// Begins the unit of work.
        /// </summary>
        public virtual void BeginUnitOfWork()
        {
            Repository.BeginUnitOfWork();

        }

        /// <summary>
        /// Saves the pending changes and completes the unit of work.
        /// </summary>
        public virtual void CompleteUnitOfWork()
        {
            Repository.CompleteUnitOfWork();
        }

        /// <summary>
        /// Completes the unit of work and optionally saves the changes to the datastore.
        /// </summary>
        /// <param name="saveChanges">if set to <c>true</c> [save changes].</param>
        public virtual void CompleteUnitOfWork(bool saveChanges)
        {
            Repository.CompleteUnitOfWork(saveChanges);
        }

        /// <summary>
        /// Attaches the specified entity to the current Unit of Work.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Attach(EntityBase entity)
        {
            Repository.Attach(entity);
        }


        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public virtual IDbTransaction BeginTransaction()
        {
            return Repository.BeginTransaction();
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        public virtual IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Repository.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Adds the specified new entity to the unit of work.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add(EntityBase entity)
        {
            Repository.Add(entity);
        }

        #endregion

        
    }
}