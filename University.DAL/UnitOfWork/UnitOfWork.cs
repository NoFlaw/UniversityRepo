using System;
using System.Data.Entity;
using University.DAL.Models;
using University.DAL.Repository;

namespace University.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly UniversityContext _context;

        public UnitOfWork(UniversityContext context)
        {
            _context = context;
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
                    GC.SuppressFinalize(this);
                }
            }
            _disposed = true;
        }

    }

}
