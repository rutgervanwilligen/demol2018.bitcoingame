using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();

        void Add(T t);
        void Update(T t);
        void Delete(T t);
        void SaveChanges();
    }
}
