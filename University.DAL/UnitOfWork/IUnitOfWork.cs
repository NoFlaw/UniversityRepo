using System;
using System.Threading;
using System.Threading.Tasks;

namespace University.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task<int> SaveAsync();
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
