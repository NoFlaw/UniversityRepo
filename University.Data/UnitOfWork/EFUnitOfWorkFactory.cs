using System;
using System.Data.Entity;
using University.Data.UnitOfWork.Base;

namespace University.Data.UnitOfWork
{
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static Func<DbContext> _objectContextDelegate;
        private static readonly Object LockObject = new object();

        public static void SetObjectContext(Func<DbContext> objectContextDelegate)
        {
            _objectContextDelegate = objectContextDelegate;
        }

        public IUnitOfWork Create()
        {
            DbContext context;

            lock (LockObject)
            {
                context = _objectContextDelegate();
            }

            return new EFUnitOfWork(context);

        }
    }
}
