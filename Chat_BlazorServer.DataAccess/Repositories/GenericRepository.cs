using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;

namespace Chat_BlazorServer.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
    {
        protected readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void Add(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predecate)
        {
            return _context.Set<TEntity>().Where(predecate);
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public void Remove(TEntity item)
        {
            _context.Set<TEntity>().Remove(item);
        }
    }
}
