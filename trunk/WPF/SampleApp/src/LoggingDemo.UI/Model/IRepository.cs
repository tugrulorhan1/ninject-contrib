#region Using Directives

using System;
using System.Collections.Generic;
using System.Data;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Querying;

#endregion

namespace LoggingDemo.UI.Model
{
    public interface IRepository
    {
        /// <summary>
        /// Finds all the entities of the given type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        IList<Entity> Find(Type entityType);

        /// <summary>
        /// Finds all the entities of the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IList<TEntity> Find<TEntity>() where TEntity : EntityBase;

        /// <summary>
        /// Finds all the entities of the given type that satisfy the given conditions.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IList<Entity> Find(Type entityType, QueryExpression expression);

        /// <summary>
        /// Finds all the entities of the given type that satisfy the given conditions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The query expression.</param>
        /// <returns></returns>
        IList<TEntity> Find<TEntity>(QueryExpression expression) where TEntity : EntityBase;

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified conditions.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="order">The order.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        IList<Entity> Find(Type entityType, QueryExpression expression, Order order, Page page);

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified conditions.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="order">The order.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        IList<TEntity> Find<TEntity>(QueryExpression expression, Order order, Page page) where TEntity : EntityBase;

        /// <summary>
        /// Finds all the entities yielded by the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IList<Entity> Find(Query query);

        /// <summary>
        /// Finds all the entities of the given type yielded by the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IList<TEntity> Find<TEntity>(Query query) where TEntity : EntityBase;

        /// <summary>
        /// Finds one entity by Id.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Entity FindOne(Type entityType, int id);

        /// <summary>
        /// Finds one entity by Id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(int id) where TEntity : EntityBase;

        /// <summary>
        /// Finds the first entity returned for the specified expression.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        Entity FindOne(Type entityType, QueryExpression expression);

        /// <summary>
        /// Finds the first entity returned for the specified expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The query expression.</param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(QueryExpression expression) where TEntity : EntityBase;

        /// <summary>
        /// Finds the first entity returned for the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Entity FindOne(Query query);

        /// <summary>
        /// Finds the first entity returned for the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(Query query) where TEntity : EntityBase;

        /// <summary>
        /// Returns the number of entities in the database for the specified query
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        long Count(Query query);

        /// <summary>
        /// Returns the number of all the entities in the database
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        long Count<TEntity>() where TEntity : EntityBase;

        /// <summary>
        /// Returns the number of entities in the database for the specified query
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        long Count<TEntity>(Query query) where TEntity : EntityBase;

        /// <summary>
        /// Projects the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IDataReader Project(Query query);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entityToRemove">The entity to remove.</param>
        void Remove(EntityBase entityToRemove);

        /// <summary>
        /// Saves the pending changes and flushes the unit of work
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves the pending changes and optionally resets the unit of work.
        /// </summary>
        /// <param name="resetUnitOfWork">if set to <c>true</c> [reset unit of work].</param>
        void SaveChanges(bool resetUnitOfWork);

        /// <summary>
        /// Begins the unit of work.
        /// </summary>
        void BeginUnitOfWork();

        /// <summary>
        /// Saves the pending changes and completes the unit of work.
        /// </summary>
        void CompleteUnitOfWork();

        /// <summary>
        /// Completes the unit of work and optionally saves the changes to the datastore.
        /// </summary>
        /// <param name="saveChanges">if set to <c>true</c> [save changes].</param>
        void CompleteUnitOfWork(bool saveChanges);

        /// <summary>
        /// Attaches the specified entity to the current Unit of Work.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Attach(EntityBase entity);

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        IDbTransaction BeginTransaction();

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Adds the specified new entity to the unit of work.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(EntityBase entity);
    }
}