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
        private GameService GameService { get; set; }

        public RoundService(GameService gameService, BitcoinGameDbContext dbContext)
        {
            GameService = gameService;
            RoundRepository = new RoundRepository(dbContext);
        }

        public Round StartNewRound(TimeSpan roundLength)
        {
            var currentGame = GameService.FindCurrentGame() ?? GameService.StartNewGame();

            var round = new Round {
                Game = currentGame,
                RoundNumber = currentGame.Rounds.Any() ? currentGame.Rounds.Max(x => x.RoundNumber) + 1 : 1
            };
            
            round.Start(roundLength);

            RoundRepository.Add(round.ToEntity());
            RoundRepository.SaveChanges();

            return round;
        }

        public Round GetCurrentRound()
        {
            var currentGame = GameService.FindCurrentGame() ?? GameService.StartNewGame();

            if (!currentGame.Rounds.Any())
            {
                return null;
            }

            return currentGame.Rounds.OrderByDescending(x => x.RoundNumber).First();
        }
    }
}