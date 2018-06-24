using DeMol2018.BitcoinGame.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class WalletRepository : BitcoinGameBaseRepository<WalletEntity>
    {
        public WalletRepository(DbContext context)
            : base(context) { }
    }
}
