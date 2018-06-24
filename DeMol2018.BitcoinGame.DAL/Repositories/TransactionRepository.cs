using DeMol2018.BitcoinGame.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class TransactionRepository : BitcoinGameBaseRepository<TransactionEntity>
    {
        public TransactionRepository(DbContext context)
            : base(context) { }
    }
}
