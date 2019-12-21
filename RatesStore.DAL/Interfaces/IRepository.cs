using RatesStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesStore.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity item);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        int GetCount();
        void AddRange(IEnumerable<TEntity> item);
        void Remove(TEntity item);
        void Remove(int id);
        void RemoveAll();
        void Update(TEntity item);
    }

}
