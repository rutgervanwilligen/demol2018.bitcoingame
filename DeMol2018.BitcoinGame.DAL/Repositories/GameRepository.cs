using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class GameRepository : BitcoinGameBaseRepository<GameEntity>
    {
        public GameRepository(BitcoinGameDbContext context)
            : base(context)
        {
            AddDefaultIncludes(x => x.Rounds);
        }

        public GameEntity FindCurrentGame()
        {
            return GetAll().FirstOrDefault(x => x.IsCurrentGame);
        }
    }
}
