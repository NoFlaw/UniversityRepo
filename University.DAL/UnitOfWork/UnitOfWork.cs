using System;
using University.DAL.Models;
using University.DAL.Repository;

namespace University.DAL.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private bool _disposed;

        private readonly UniversityContext _context = new UniversityContext();
        
        private GenericRepository<Course> _courseRepository;
        private GenericRepository<Department> _departmentRepository;
        private GenericRepository<Enrollment> _enrollmentRepository;
        private GenericRepository<Instructor> _instructorRepository;
        private GenericRepository<OfficeAssignment> _officeAssignmentRepository; 
        private GenericRepository<Student> _studentRepository;
        
        public GenericRepository<Course> CourseRepository
        {
            get { return _courseRepository ?? (_courseRepository = new GenericRepository<Course>(_context)); }
        }

        public GenericRepository<Department> DepartmentRepository
        {
            get { return _departmentRepository ?? (_departmentRepository = new GenericRepository<Department>(_context)); }
        }

        public GenericRepository<Enrollment> EnrollmentRepository
        {
            get { return _enrollmentRepository ?? (_enrollmentRepository = new GenericRepository<Enrollment>(_context)); }
        }

        public GenericRepository<Instructor> InstructorRepository
        {
            get { return _instructorRepository ?? (_instructorRepository = new GenericRepository<Instructor>(_context)); }
        }

        public GenericRepository<OfficeAssignment> OfficeAssignmentRepository
        {
            get { return _officeAssignmentRepository ?? (_officeAssignmentRepository = new GenericRepository<OfficeAssignment>(_context)); }
        }

        public GenericRepository<Student> StudentRepository
        {
            get { return _studentRepository ?? (_studentRepository = new GenericRepository<Student>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
