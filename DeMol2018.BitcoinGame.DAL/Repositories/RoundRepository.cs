using DeMol2018.BitcoinGame.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class RoundRepository : BitcoinGameBaseRepository<RoundEntity>
    {
        public RoundRepository(DbContext context)
            : base(context) { }
    }
}
