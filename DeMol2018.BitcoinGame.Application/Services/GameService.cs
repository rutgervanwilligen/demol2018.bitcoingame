using System;
using System.Linq;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class GameService
    {
        private GameRepository GameRepository { get; }

        public GameService(BitcoinGameDbContext dbContext)
        {
            GameRepository = new GameRepository(dbContext);
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
            
            return gameEntity.ToDomainModel();
        }

        public void StartNewRound(TimeSpan roundLength)
        {
            var currentGame = GameRepository.FindCurrentGame();

            if (currentGame == null)
            {
                return;
            }

            var newRound = new Round {
                GameId = currentGame.Id,
                RoundNumber = currentGame.Rounds.Any() ? currentGame.Rounds.Max(x => x.RoundNumber) + 1 : 1
            };

            MarkAllRoundsInGameFinished(currentGame);

            newRound.Start(roundLength);

            currentGame.Rounds.Add(newRound.ToEntity());

            GameRepository.SaveChanges();
        }

        private void MarkAllRoundsInGameFinished(GameEntity game)
        {
            var now = DateTime.UtcNow;

            foreach (var round in game.Rounds)
            {
                if (round.EndTime > now)
                {
                    round.EndTime = now;
                }
            }
        }

        private void InactivateAllOldGames()
        {
            var oldGames = GameRepository.GetAll().Where(x => x.IsCurrentGame);

            foreach (var oldGame in oldGames)
            {
                oldGame.IsCurrentGame = false;
            }
        }

        public void FinishCurrentGame()
        {
            var currentGame = GameRepository.FindCurrentGame();

            MarkAllRoundsInGameFinished(currentGame);

            currentGame.HasFinished = true;

            GameRepository.SaveChanges();
        }
    }
}