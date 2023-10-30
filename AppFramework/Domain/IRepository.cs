using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppFramework.Domain
{
    public interface IRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(TKey id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SoftDeleteAsync(TEntity entity);


        bool Exists(Expression<Func<TEntity, bool>> expression);
        void SaveChanges();
    }

    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        //public IQueryable<TEntity> GetAll()
        //{
        //    return _dbSet.AsQueryable();
        //}
        //public List<TEntity> GetAll()
        //{
        //    return _context.Set<TEntity>().ToList();
        //}

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);

        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        //public async Task<TEntity> Addsync(TEntity entity)
        //{
        //    await _dbSet.AddAsync(entity);
        //    await _dbContext.SaveChangesAsync();
        //    return entity;
        //}


        public async Task UpdateAsync(TEntity entity)
        {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            //_dbContext.Update(entity);
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task DeleteAsync(TEntity entity)
        //{
        //    _dbSet.Remove(entity);
        //    await SaveChangesAsync();
        //}
        //add inventory
        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Any(expression);
            //Or
            //    return _dbSet.Any(expression);
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        //private async Task SaveChangesAsync()
        //{
        //    await _dbContext.SaveChangesAsync();
        //}
    }

}