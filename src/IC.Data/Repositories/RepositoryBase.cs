using IC.Data.Context;
using IC.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IC.Data.Repositories
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected InventoryControlContext ctx = new InventoryControlContext();

        public void Add(TEntity obj)
        {
            ctx.Set<TEntity>().Add(obj);
            ctx.SaveChanges();
        }

        public void Dispose()
        {
            ctx.Dispose();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return ctx.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return ctx.Set<TEntity>().Find(id);
        }

        public void Remove(TEntity obj)
        {
            ctx.Set<TEntity>().Remove(obj);
            ctx.SaveChanges();
        }

        public void Update(TEntity obj)
        {
            ctx.Entry(obj).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}