using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly UniversityContext _context = new UniversityContext();

        /// <summary>
        ///     Collection of All Entities within the context.
        /// </summary>
        private readonly DbSet<T> _dbSet;

        private readonly Guid _instanceId;

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
            
            var unitOfWork = new UnitOfWork.UnitOfWork(_context);
            
            if (unitOfWork == null)
                throw new ArgumentException("UnitOfWorkContext is null");

            _dbSet = unitOfWork.GetDbSet<T>();

            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual T Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual async Task<T> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync( keyValues);
        }

        public virtual async Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        internal async Task<IEnumerable<T>> GetAsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           List<Expression<Func<T, object>>> includeProperties = null,
           int? page = null,
           int? pageSize = null)
        {
            return this.Get(filter, orderBy, includeProperties).AsEnumerable();
        }

        internal IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }




        public virtual IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
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

        public T FindById(object id)
        {
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
        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }


        /// <summary>
        ///     Removes specified entity from the entity collection by ID in preparation for a delete.
        ///     No changes are persisted to the database until the Save is called.
        /// </summary>
        /// <param name="id">The entity.</param>
        public virtual void Delete(object id)
        {
            var entity = FindById(id);
            Delete(entity);
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
