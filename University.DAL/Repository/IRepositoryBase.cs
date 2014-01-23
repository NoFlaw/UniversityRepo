using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace University.DAL.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Gets a count of TEntity
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Gets a count of TEntity
        /// </summary>
        /// <returns></returns>
        int CountWhere(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Finds the Entity by the primary key id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        T FindById(object id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///     Gets all of the entities from the database, with optional set of eager includes.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///     Gets all of the entities from the database.
        /// </summary>
        /// <returns></returns>
        DbQuery<T> Query();

        /// <summary>
        ///     Gets all of the entities from the database that satisfy a predicate condition
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Removes specified entity from the entity collection in preparation for a delete.
        ///     No changes are persisted to the database until the Save is called.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(T entity);

    }
}
