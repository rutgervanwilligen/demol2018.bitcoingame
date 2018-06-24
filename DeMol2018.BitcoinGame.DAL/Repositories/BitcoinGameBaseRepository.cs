using System;
using System.Linq;
using System.Linq.Expressions;
using DeMol2018.BitcoinGame.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public abstract class BitcoinGameBaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        protected BitcoinGameBaseRepository(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public T FindBy(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).SingleOrDefault();
        }

        public T GetBy(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).Single();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T t)
        {
            _dbSet.Update(t);
        }

        public void Delete(T t)
        {
            _dbSet.Remove(t);
        }
    }
}
