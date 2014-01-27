using System;
using System.Collections;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using University.DAL.Models;
using University.DAL.Repository;

namespace University.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        
        #region private variables
        
        private bool _disposed;
        private readonly UniversityContext _context = new UniversityContext();
        private EfRepository<Course> _courseRepository;
        private EfRepository<Department> _departmentRepository;
        private EfRepository<Enrollment> _enrollmentRepository;
        private EfRepository<Instructor> _instructorRepository;
        private EfRepository<OfficeAssignment> _officeAssignmentRepository;
        private readonly Guid _instanceId;
        private EfRepository<Student> _studentRepository;
        private Hashtable _repositories;

        public UnitOfWork(UniversityContext context)
        {
            _instanceId = Guid.NewGuid();
        }

        #endregion 
        
        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : EntityBase
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryBase<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(EfRepository<>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));

            return (IRepositoryBase<TEntity>)_repositories[type];
        }

        public EfRepository<Course> CourseRepository
        {
            get { return _courseRepository ?? (_courseRepository = new EfRepository<Course>(_context)); }
        }

        public EfRepository<Department> DepartmentRepository
        {
            get { return _departmentRepository ?? (_departmentRepository = new EfRepository<Department>(_context)); }
        }

        public EfRepository<Enrollment> EnrollmentRepository
        {
            get { return _enrollmentRepository ?? (_enrollmentRepository = new EfRepository<Enrollment>(_context)); }
        }

        public EfRepository<Instructor> InstructorRepository
        {
            get { return _instructorRepository ?? (_instructorRepository = new EfRepository<Instructor>(_context)); }
        }

        public EfRepository<OfficeAssignment> OfficeAssignmentRepository
        {
            get { return _officeAssignmentRepository ?? (_officeAssignmentRepository = new EfRepository<OfficeAssignment>(_context)); }
        }

        public EfRepository<Student> StudentRepository
        {
            get { return _studentRepository ?? (_studentRepository = new EfRepository<Student>(_context)); }
        }

        internal DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {        
                if (!_disposed && disposing)
                {
                    _context.Dispose();
                    
                }
           
            _disposed = true;
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

    }

}
