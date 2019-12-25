using System;

namespace Student.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        void CommitTransaction();
        void StartTransaction();
    }
}
