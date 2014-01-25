using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace University.DAL.Repository
{
    /// <summary>
    ///     Base repository class from which all other repositories must inherit.
    ///     Common functionality in the Entity Framework DbContext class is invoked.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IRepositoryBase<T> where T : class
    {
        private static readonly PropertyIncluder<T> Includer = new PropertyIncluder<T>();

        /// <summary>
        ///     The Entity Framework DbContext object.
        /// </summary>
        private readonly UniversityContext _context;

        private readonly DbSet<T> _dbSet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EfRepository{T}" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentException">context is null</exception>
        protected internal EfRepository(UniversityContext context)
        {
            _context = context;

            if (context == null)
                throw new ArgumentException("context is null");
            
            var unitOfWork = new UnitOfWork.UnitOfWork(context);
            
            if (unitOfWork == null)
                throw new ArgumentException("UnitOfWorkContext is null");

            _dbSet = unitOfWork.GetDbSet<T>();
        }

        /// <summary>
        ///     Gets a count of TEntity
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _dbSet.Count();
        }

        public int CountWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        /// <summary>
        ///     The base version uses the built in EntityFramework Find method.
        ///     The params expressions are ignored.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public virtual T FindById(object id, params Expression<Func<T, object>>[] includes)
        {
            if (includes.Any())
            {
                throw new ArgumentException(
                    "Includes are not handled for FindById when using default implementation of EfRepository.  Please override FindById method in derived class",
                    "includes");
            }
            if (Includer.HasIncludes)
            {
                throw new ArgumentException(
                    string.Format("Member of {0} marked with Include attribute. Please override FindById method in derived class", typeof(T).Name),
                    "includes");
            }
            return _dbSet.Find(id);
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Query();
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        ///     Gets all of the entities from the database.
        /// </summary>
        /// <returns></returns>
        public virtual DbQuery<T> Query()
        {
            return Includer.BuildQuery(_dbSet);
        }

        public virtual IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return Query().Where(predicate);
        }


        /// <summary>
        ///     Removes specified entity from the entity collection in preparation for a delete.
        ///     No changes are persisted to the database until the Save is called.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

    }

    public class PropertyIncluder<T> where T : class
    {
        private readonly Func<DbQuery<T>, DbQuery<T>> _includeMethod;
        private readonly HashSet<Type> _visitedTypes;
        public readonly bool HasIncludes;

        public PropertyIncluder()
        {
            //Recursively get properties to include
            _visitedTypes = new HashSet<Type>();
            var propsToLoad = GetPropsToLoad(typeof (T)).ToArray();

            HasIncludes = propsToLoad.Any();

            _includeMethod = d => propsToLoad.Aggregate(d, (current, prop) => current.Include(prop));
        }

        private IEnumerable<string> GetPropsToLoad(Type type)
        {
            _visitedTypes.Add(type);
            var propsToLoad = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof (IncludeAttribute), true).Any());

            foreach (var prop in propsToLoad)
            {
                yield return prop.Name;

                if (_visitedTypes.Contains(prop.PropertyType))
                    continue;

                foreach (var subProp in GetPropsToLoad(prop.PropertyType))
                {
                    yield return prop.Name + "." + subProp;
                }
            }
        }

        public DbQuery<T> BuildQuery(DbSet<T> dbSet)
        {
            return _includeMethod(dbSet);
        }
    }

}
