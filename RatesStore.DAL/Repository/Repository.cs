using RatesStore.DAL.Interfaces;
using RatesStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        DbContext context;
        DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsNoTracking();
        }

        public int GetCount()
        {
            return dbSet.AsNoTracking().ToList().Count;
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return dbSet.Where(predicate);
        }
        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Add(TEntity item)
        {
            dbSet.Add(item);
        }

        public bool Any(Func<TEntity, bool> predicate)
        {
            return dbSet.Any<TEntity>(predicate);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
       
        public void Remove(TEntity item)
        {
            dbSet.Remove(item);
        }
        public void Remove(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }
        public void RemoveAll()
        {
            dbSet.RemoveRange(dbSet);         
        }
    }
}
