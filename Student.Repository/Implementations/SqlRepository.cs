using Student.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Student.Repository.Implementations
{
    public abstract class SqlRepository<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        private readonly IDbContext context;
        private DbSet<TEntity> dbSetTEntity;
        public SqlRepository(IDbContext context)
        {
            this.context = context;
            this.dbSetTEntity = this.context.Set<TEntity>();
            #if DEBUG
                        //this.context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            #endif
        }
        
        public virtual void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("Entity Null");

                dbSetTEntity.Attach(entity);
                context.Entry<TEntity>(entity).State = EntityState.Modified;
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("Entity Null");
                dbSetTEntity.Add(entity);
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void InsertList(List<TEntity> entityList)
        {
            try
            {
                if (entityList == null)
                    throw new ArgumentNullException("Entity Null");
                dbSetTEntity.AddRange(entityList);
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("Entity Null");
                dbSetTEntity.Remove(entity);
                SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return this.context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
#if DEBUG
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
#endif
            }
            return -1;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        public virtual TEntity GetById(int id)
        {
            return dbSetTEntity.Find(id);
        }
        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbSetTEntity.FindAsync(id, cancellationToken);
        }

        public IQueryable<TEntity> Table
        {
            get { return dbSetTEntity.AsQueryable(); }
        }

        public T GetCurrentDbContext<T>()
        {
            return (T)context;
        }

        public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                if (where == null)
                    throw new ArgumentNullException("Expression Null");
                return dbSetTEntity.Where(where).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken)
        {
            try
            {
                if (where == null)
                    throw new ArgumentNullException("Expression Null");
                return await dbSetTEntity.Where(where).ToListAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DbQuery<TEntity> Include(string path)
        {
            return dbSetTEntity.Include(path);
        }
    }

    public class StudentDBRepo<TEntity> : SqlRepository<TEntity>, IStudentRepo<TEntity> where TEntity : class
    {
        private DbSet<TEntity> dbSetTEntity;
        public StudentDBRepo(IStudentContext _dbContext)
            : base(_dbContext)
        {
            dbSetTEntity = _dbContext.Set<TEntity>();
        }
        public IQueryable<TEntity> TableObject
        {
            get { return dbSetTEntity.AsQueryable(); }
        }

    }
}
