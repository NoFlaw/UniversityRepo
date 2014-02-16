using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using University.Data.Repository.Base;
using University.Data.UnitOfWork;
using University.Data.UnitOfWork.Base;

namespace University.Data.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        //Initialization
        #region Private Members

        private DbContext _context;
        private IDbSet<T> _dbSet;

        #endregion

        //Hooking Up
        #region Protected Members

        protected DbContext Context
        {
            get { return _context ?? (_context = GetCurrentUnitOfWork<EFUnitOfWork>().Context); }
        }

        protected IDbSet<T> DataSet
        {
            get { return _dbSet ?? (_dbSet = Context.Set<T>()); }
        }

        #endregion

        //Work Methods
        #region Public Members
        //TODO: All this needs to be fully tested


        public void Update(T entity)
        {
            var entry = Context.Entry(entity);
            DataSet.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public IQueryable<T> GetQuery()
        {
            return DataSet.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return GetQuery().ToList();
        }

        public T Single(Expression<Func<T, bool>> where = null)
        {
            return (where == null)
                       ? DataSet.SingleOrDefault()
                       : DataSet.SingleOrDefault(where);
        }

        public T First(Expression<Func<T, bool>> where = null)
        {
            return (where == null)
                       ? DataSet.First()
                       : DataSet.First(where);
        }

        public int Count
        {
            get { return DataSet.Count(); }
        }

        public int GetCountFor(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null)
                       ? DataSet.Count()
                       : DataSet.Count(predicate);
        }

        public T GetById(object id)
        {
            return DataSet.Find(id);
        }

        public bool Exist(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null) ? DataSet.Any() : DataSet.Any(predicate);
        }

        public IQueryable<T> GetObjectGraph(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DataSet;

            if (filter != null)
                query = query.Where(filter);


            if (!String.IsNullOrWhiteSpace(includeProperties))
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null) ? DataSet : DataSet.Where(predicate).AsQueryable();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null ? DataSet.Where(filter).AsQueryable() : DataSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<T, bool>> predicate = null)
        {
            return (predicate != null) && DataSet.Count(predicate) > 0;
        }

        public T Find(params object[] keys)
        {
            return DataSet.Find(keys);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return DataSet.FirstOrDefault(predicate);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DataSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                DataSet.Attach(entity);

            DataSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = Filter(predicate);
            foreach (var entity in entitiesToDelete)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    DataSet.Attach(entity);
                }
                DataSet.Remove(entity);
            }
        }

        public IQueryable<T> FindIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            if (includeProperties != null)
                foreach (var include in includeProperties)
                    DataSet.Include(include);

            return DataSet.AsQueryable();
        }

        /// <summary>
        ///     Adds specified entity to the collection.
        ///     No changes are persisted to the database until the Save is called.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Add(T entity)
        {
            DataSet.Add(entity);
        }

        public void Attach(T entity)
        {
            DataSet.Attach(entity);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        #endregion

        //UOW
        #region UnitOfWork Implementation

        public TUnitOfWork GetCurrentUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
        {
            return (TUnitOfWork)UnitOfWork.UnitOfWork.Current;
        }

        #endregion

    }
}
