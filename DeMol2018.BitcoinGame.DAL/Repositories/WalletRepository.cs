using DeMol2018.BitcoinGame.DAL.Entities;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class WalletRepository : BitcoinGameBaseRepository<WalletEntity>
    {
        public WalletRepository(BitcoinGameDbContext context)
            : base(context)
        {
            AddDefaultIncludes(x => x.IncomingTransactions);
            AddDefaultIncludes(x => x.OutgoingTransactions);
            AddDefaultIncludes(x => x.Player);
        }
    }
}
