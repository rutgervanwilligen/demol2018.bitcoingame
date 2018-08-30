using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class WalletService
    {
        private WalletRepository WalletRepository { get; }
        private const int EuroBalanceAtNewGame = 0;
        private const int MinimalAmountForStuivertjeWisselen = 500;
        private const int EuroPenaltyWhenNoMoneyIsWonInRound = 500;

        public WalletService(BitcoinGameDbContext dbContext)
        {
            WalletRepository = new WalletRepository(dbContext);
        }

        public Wallet GetWalletByGameIdAndPlayerId(Guid gameId, Guid playerId)
        {
            return WalletRepository
                .GetBy(x => x.PlayerId == playerId
                         && x.GameId == gameId)
                .ToDomainModel();
        }

        public List<Wallet> GetNonPlayerWalletsByGameId(Guid gameId)
        {
            return WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                            && x.Type != WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel())
                .ToList();
        }

        public int GetMoneyWonSoFarInGameIdUpUntilRound(Guid gameId, int? roundNumber)
        {
            if (!roundNumber.HasValue)
            {
                return EuroBalanceAtNewGame;
            }

            var moneyWonSoFar = EuroBalanceAtNewGame;

            for (var i = 1; i <= roundNumber.Value; i++)
            {
                var moneyWonInRound = GetNonPlayerWalletMoneyWonInRound(gameId, i)
                                       + GetPlayerWalletMoneyWonInRound(gameId, i);

                if (moneyWonInRound == 0)
                {
                    moneyWonSoFar -= EuroPenaltyWhenNoMoneyIsWonInRound;
                }
                else
                {
                    moneyWonSoFar += moneyWonInRound;
                }
            }

            return moneyWonSoFar;
        }

        private int GetNonPlayerWalletMoneyWonInRound(Guid gameId, int roundNumber)
        {
            return WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                            && x.Type != WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel())
                .ToList()
                .Sum(x => x.GetMoneyWonInRound(roundNumber));
        }

        private int GetPlayerWalletMoneyWonInRound(Guid gameId, int roundNumber)
        {
            var eligibleTransactions = WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                            && x.Type == WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel()).ToList()
                .SelectMany(x => x.OutgoingTransactions).ToList()
                .Where(y => y.RoundNumber == roundNumber
                            && y.ReceiverWalletId.HasValue
                            && y.Amount >= MinimalAmountForStuivertjeWisselen)
                .ToList();

            return MoneySwap.GetMoneySwapResultOfTransactions(eligibleTransactions.ToList());
        }

        public IEnumerable<JokerWinner> GetJokerWinnersInGame(Guid gameId)
        {
            var currentGamePlayerWallets = WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                         && x.Type == WalletEntity.WalletType.PlayerWallet.ToString())
                .ToList();

            var jokerWinners = currentGamePlayerWallets
                .Select(x => new JokerWinner {
                    PlayerId = x.Player.Id,
                    PlayerName = x.Player.Name,
                    NumberOfJokersWon = 0
                })
                .ToList();

            var jokerWinnersFromJokerWallet = GetJokerWinnersFromJokerWallet(gameId, currentGamePlayerWallets).ToList();
            var jokerWinnersFromHighestBalance = GetJokerWinnersFromHighestBalance(currentGamePlayerWallets).ToList();

            foreach (var jokerWinner in jokerWinners)
            {
                var jokersWonJokerWallet = jokerWinnersFromJokerWallet
                    .SingleOrDefault(x => x.PlayerId == jokerWinner.PlayerId)?.NumberOfJokersWon;

                var jokersWonFromHighestBalance = jokerWinnersFromHighestBalance
                    .SingleOrDefault(x => x.PlayerId == jokerWinner.PlayerId)?.NumberOfJokersWon;

                if (jokersWonJokerWallet.HasValue)
                {
                    jokerWinner.NumberOfJokersWon += jokersWonJokerWallet.Value;
                }

                if (jokersWonFromHighestBalance.HasValue)
                {
                    jokerWinner.NumberOfJokersWon += jokersWonFromHighestBalance.Value;
                }
            }

            return jokerWinners;
        }

        private IEnumerable<JokerWinner> GetJokerWinnersFromJokerWallet(Guid gameId, IEnumerable<WalletEntity> playerWallets)
        {
            var jokerWallet = WalletRepository
                .GetAll()
                .Single(x => x.GameId == gameId &&
                             x.Type == WalletEntity.WalletType.JokerWallet.ToString())
                .ToDomainModel() as JokerWallet;

            if (jokerWallet == null)
            {
                throw new Exception("No JokerWallet found in game");
            }

            var jokerWinners = jokerWallet.GetJokerWinners();

            return jokerWinners.Select(x => {
                var player = playerWallets.Single(y => y.Id == x.SenderWalletId).Player;
                return new JokerWinner {
                    NumberOfJokersWon = x.NumberOfJokersWon,
                    PlayerId = player.Id,
                    PlayerName = player.Name
                };
            });
        }

        private IEnumerable<JokerWinner> GetJokerWinnersFromHighestBalance(IEnumerable<WalletEntity> playerWallets)
        {
            var playerWalletEntities = playerWallets.ToList();

            var walletsSortedByBalance = playerWalletEntities
                .Select(x => x.ToDomainModel())
                .ToList()
                .OrderByDescending(x => x.GetFinalBalance())
                .ToList();

            var highestEndBalance = walletsSortedByBalance.First().GetFinalBalance();
            var walletsWithHighestBalance =
                walletsSortedByBalance.Where(x => x.GetFinalBalance() == highestEndBalance).ToList();

            if (walletsWithHighestBalance.Count > 1)
            {
                return walletsWithHighestBalance.Select(x =>
                {
                    var player = playerWalletEntities.Single(y => y.Id == x.Id).Player;
                    return new JokerWinner {
                        PlayerId = player.Id,
                        PlayerName = player.Name,
                        NumberOfJokersWon = 1
                    };
                });
            }

            var winner = playerWalletEntities.Single(y => y.Id == walletsWithHighestBalance.Single().Id).Player;

            return new List<JokerWinner> { new JokerWinner {
                PlayerId = winner.Id,
                PlayerName = winner.Name,
                NumberOfJokersWon = 2
            }};
        }
    }
}