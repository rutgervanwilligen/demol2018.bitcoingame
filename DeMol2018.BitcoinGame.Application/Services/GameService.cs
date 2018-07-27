using System.Linq;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class GameService
    {
        private GameRepository GameRepository { get; set; }
        private RoundRepository RoundRepository { get; set; }

        public GameService(BitcoinGameDbContext dbContext)
        {
            GameRepository = new GameRepository(dbContext);
            RoundRepository = new RoundRepository(dbContext);
        }
        
        public Game FindCurrentGame()
        {
            return GameRepository.FindCurrentGame()?.ToDomainModel();
        }

        public Game StartNewGame()
        {
            var newGame = new Game();
            
            var gameEntity = newGame.ToEntity();

            InactivateAllOldGames();

            GameRepository.Add(gameEntity);
            GameRepository.SaveChanges();

            return newGame;
        }

        private void InactivateAllOldGames()
        {
            var oldGames = GameRepository.GetAll().Where(x => !x.HasFinished);

            foreach (var oldGame in oldGames)
            {
                oldGame.HasFinished = true;
            }
        }
    }
}