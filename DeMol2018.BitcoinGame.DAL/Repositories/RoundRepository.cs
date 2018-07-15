using DeMol2018.BitcoinGame.DAL.Entities;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class RoundRepository : BitcoinGameBaseRepository<RoundEntity>
    {
        public RoundRepository(BitcoinGameDbContext context)
            : base(context) { }
    }
    
    
}
