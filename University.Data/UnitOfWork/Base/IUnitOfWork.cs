using System;

namespace University.Data.UnitOfWork.Base
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
