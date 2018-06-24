using DeMol2018.BitcoinGame.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class PlayerRepository : BitcoinGameBaseRepository<PlayerEntity>
    {
        public PlayerRepository(DbContext context)
            : base(context) { }
    }
}
