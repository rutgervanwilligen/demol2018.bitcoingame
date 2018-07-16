using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class GameService
    {
        private GameRepository GameRepository { get; set; }

        public GameService(BitcoinGameDbContext dbContext)
        {
            GameRepository = new GameRepository(dbContext);
        }
        
        public Game StartNewGame()
        {
            var newGame = new Game();

            var gameEntity = newGame.ToEntity();
            
            GameRepository.Add(gameEntity);
            GameRepository.SaveChanges();

            return newGame;
        }
        
        public Game FindCurrentGame()
        {
            return GameRepository.FindCurrentGame()?.ToDomainModel();
        }
    }
}