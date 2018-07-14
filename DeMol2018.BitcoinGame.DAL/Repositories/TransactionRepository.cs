using DeMol2018.BitcoinGame.DAL.Entities;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class TransactionRepository : BitcoinGameBaseRepository<TransactionEntity>
    {
        public TransactionRepository(BitcoinGameDbContext context)
            : base(context) { }
    }
}
