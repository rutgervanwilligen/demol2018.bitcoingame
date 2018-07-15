using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Repositories;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class RoundService
    {
        private RoundRepository RoundRepository { get; set; }

        public RoundService(BitcoinGameDbContext dbContext)
        {
            RoundRepository = new RoundRepository(dbContext);
        }
        
        
    }
}