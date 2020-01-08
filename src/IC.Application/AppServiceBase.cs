using IC.Application.Interfaces;
using IC.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace IC.Application
{
    public class AppServiceBase<TEntity> : IAppServiceBase<TEntity> where TEntity : class
    {
        private readonly IServiceBase<TEntity> _serviceBase;

        public AppServiceBase(IServiceBase<TEntity> serviceBase)
        {
            _serviceBase = serviceBase;
        }

        public virtual void Add(TEntity obj)
        {
            _serviceBase.Add(obj);
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _serviceBase.GetAll();
        }

        public TEntity GetById(int id)
        {
            return _serviceBase.GetById(id);
        }

        public virtual void Remove(TEntity obj)
        {
            _serviceBase.Remove(obj);
        }

        public virtual void Update(TEntity obj)
        {
            try
            {
                _serviceBase.Update(obj);
            }
            catch
            {
                throw new Exception("There was an error with your request. Verify your data and try again.");
            }
        }
    }
}