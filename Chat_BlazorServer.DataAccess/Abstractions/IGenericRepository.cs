using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Chat_BlazorServer.DataAccess.Abstractions
{
    internal interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predecate);
        void Add(TEntity item);
        void Remove(TEntity item);
    }
}
