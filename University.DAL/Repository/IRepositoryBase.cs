using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace University.DAL.Repository
{
    public interface IRepositoryBase<T> where T : class
    {

        /// <summary>
        /// Adds specified entity from the entity collection in preparation for a addition.
        /// No changes are persisted to the database until the Save is called.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);
        
        /// <summary>
        /// //TODO:Fill In Summary 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        T Find(params object[] keyValues);
        
        /// <summary>
        /// //TODO:Fill In Summary 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<T> FindAsync(params object[] keyValues);
        
        /// <summary>
        /// //TODO:Fill In Summary 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        
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
        //T FindById(object id, params Expression<Func<T, object>>[] includes);

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
        void Delete(T entity);

        void Delete(object id);
        //void Delete(T entity);

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
            );


    }
}
