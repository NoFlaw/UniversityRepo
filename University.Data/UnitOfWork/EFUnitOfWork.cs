using System;
using System.Data.Entity;
using University.Data.UnitOfWork.Base;

namespace University.Data.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }
        private bool _disposed;

        public EFUnitOfWork(DbContext context)
        {
            Context = context;
            context.Configuration.LazyLoadingEnabled = true;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);

            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    Context.Dispose();
            _disposed = true;
        }
    }
}
