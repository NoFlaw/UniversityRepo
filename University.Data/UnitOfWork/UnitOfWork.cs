using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using StructureMap;
using University.Data.UnitOfWork.Base;

namespace University.Data.UnitOfWork
{
    public static class UnitOfWork
    {
        private const string HTTPCONTEXTKEY = "University.Data.Base.HttpContext.Key";
        private static IUnitOfWorkFactory _unitOfWorkFactory;
        private static readonly Hashtable CurrentThreads = new Hashtable();

        public static void Save()
        {
            IUnitOfWork unitOfWork = GetUnitOfWork();

            if (unitOfWork != null)
                unitOfWork.Save();
        }

        public static IUnitOfWork Current
        {
            get
            {
                IUnitOfWork unitOfWork = GetUnitOfWork();

                if (unitOfWork == null)
                {
                    _unitOfWorkFactory = ObjectFactory.GetInstance<IUnitOfWorkFactory>();
                    unitOfWork = _unitOfWorkFactory.Create();
                    SaveUnitOfWork(unitOfWork);
                }

                return unitOfWork;
            }
        }

        private static IUnitOfWork GetUnitOfWork()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(HTTPCONTEXTKEY))
                    return (IUnitOfWork)HttpContext.Current.Items[HTTPCONTEXTKEY];

                return null;
            }

            Thread thread = Thread.CurrentThread;
            if (string.IsNullOrEmpty(thread.Name))
            {
                thread.Name = Guid.NewGuid().ToString();
                return null;
            }

            lock (CurrentThreads.SyncRoot)
            {
                //TODO: Revisit this for correction of null check
                return (IUnitOfWork)CurrentThreads[Thread.CurrentThread.Name];
            }
        }

        private static void SaveUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[HTTPCONTEXTKEY] = unitOfWork;
            else
            {
                lock (CurrentThreads.SyncRoot)
                {
                    //TODO: Revisit this for correction of null check
                    CurrentThreads[Thread.CurrentThread.Name] = unitOfWork;
                }
            }
        }
    }
}
