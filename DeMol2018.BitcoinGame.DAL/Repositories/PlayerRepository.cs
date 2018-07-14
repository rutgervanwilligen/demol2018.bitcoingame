using DeMol2018.BitcoinGame.DAL.Entities;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class PlayerRepository : BitcoinGameBaseRepository<PlayerEntity>
    {
        public PlayerRepository(BitcoinGameDbContext context)
            : base(context) { }
    }
}
