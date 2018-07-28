using System;
using System.Linq;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class RoundService
    {
        private RoundRepository RoundRepository { get; set; }
        private GameRepository GameRepository { get; set; }
        private GameService GameService { get; set; }

        public RoundService(GameService gameService, BitcoinGameDbContext dbContext)
        {
            GameService = gameService;
            RoundRepository = new RoundRepository(dbContext);
            GameRepository = new GameRepository(dbContext);
        }

        public Round StartNewRound(TimeSpan roundLength)
        {
            var currentGame = GameService.FindCurrentGame() ?? GameService.StartNewGame();

            var round = new Round {
                GameId = currentGame.Id,
                RoundNumber = currentGame.Rounds.Any() ? currentGame.Rounds.Max(x => x.RoundNumber) + 1 : 1
            };

            MarkAllGamesInGameFinished(currentGame.Id);
            
            round.Start(roundLength);

            RoundRepository.Add(round.ToEntity());
            RoundRepository.SaveChanges();

            return round;
        }

        private void MarkAllGamesInGameFinished(Guid gameId)
        {
            var now = DateTime.UtcNow;
            var game = GameRepository.GetBy(x => x.Id == gameId);

            foreach (var round in game.Rounds)
            {
                if (round.EndTime > now)
                {
                    round.EndTime = now;
                }
            }
        }

        public Round GetCurrentRound()
        {
            var currentGame = GameService.FindCurrentGame() ?? GameService.StartNewGame();

            if (!currentGame.Rounds.Any(x => x.IsActive)) {
                return null;
            }

            return currentGame.Rounds.OrderByDescending(x => x.RoundNumber).First();
        }
    }
}