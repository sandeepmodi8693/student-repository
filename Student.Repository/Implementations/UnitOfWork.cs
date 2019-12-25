using Student.Repository.Contracts;
using System.Transactions;

namespace Student.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope transaction;

        public void StartTransaction()
        {
            this.transaction = new TransactionScope();
        }

        public void CommitTransaction()
        {
            this.transaction.Complete();
        }

        public void Dispose()
        {
            if (this.transaction != null)
                this.transaction.Dispose();
        }
    }
}
