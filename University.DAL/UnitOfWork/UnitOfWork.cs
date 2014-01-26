using System;
using System.Data.Entity;
using University.DAL.Models;
using University.DAL.Repository;

namespace University.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly UniversityContext _context = new UniversityContext();
        private EfRepository<Course> _courseRepository;
        private EfRepository<Department> _departmentRepository;
        private EfRepository<Enrollment> _enrollmentRepository;
        private EfRepository<Instructor> _instructorRepository;
        private EfRepository<OfficeAssignment> _officeAssignmentRepository;
        private EfRepository<Student> _studentRepository;


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
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    GC.SuppressFinalize(this);
                }
            }
            _disposed = true;
        }

    }

}
