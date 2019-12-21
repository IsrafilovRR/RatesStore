using RatesStore.DAL.EF;
using RatesStore.DAL.Interfaces;
using RatesStore.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly RatesDbContext db;
        private Hashtable repositories;
        private bool disposed = false;

        public UnitOfWork()
        {
            db = new RatesDbContext();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(TEntity).Name;
            if (repositories.ContainsKey(type)) return (IRepository<TEntity>)repositories[type];

            var repositoryType = typeof(Repository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), db);

            repositories.Add(type, repositoryInstance);
            return (IRepository<TEntity>)repositories[type];
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();            
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}