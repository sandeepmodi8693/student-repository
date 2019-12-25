using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Student.Repository.Contracts
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        System.Data.Entity.Database Database { get; }
    }
    public interface IStudentContext : IDbContext
    {
    }
}
