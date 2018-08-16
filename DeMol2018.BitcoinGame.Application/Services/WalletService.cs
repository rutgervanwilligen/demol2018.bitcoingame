using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class WalletService
    {
        private WalletRepository WalletRepository { get; }
        private const int EuroBalanceAtNewGame = 0;
        private const int MinimalAmountForStuivertjeWisselen = 500;
        private const int NumberOfJokerWalletJokerWinners = 8;
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

        public int GetNumberOfJokersWonWithEndBalance(Guid gameId, Guid walletId)
        {
            var walletsSortedByBalance = WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId)
                .Select(x => x.ToDomainModel())
                .ToList()
                .OrderByDescending(x => x.GetFinalBalance())
                .ToList();

            var highestEndBalance = walletsSortedByBalance.First().GetFinalBalance();
            var walletsWithHighestBalance =
                walletsSortedByBalance.Where(x => x.GetFinalBalance() == highestEndBalance).ToList();

            if (walletsWithHighestBalance.Count > 1)
            {
                return walletsWithHighestBalance.Select(x => x.Id).Contains(walletId) ? 1 : 0;
            }

            return walletsWithHighestBalance.Single().Id == walletId ? 2 : 0;
        }

        public int GetNumberOfJokersWonFromJokerWallet(Guid gameId, Guid walletId)
        {
            var jokerWallet = WalletRepository
                .GetAll()
                .Single(x => x.GameId == gameId
                          && x.Type == WalletEntity.WalletType.JokerWallet.ToString())
                .ToDomainModel();

            var winningTransactions = jokerWallet
                .IncomingTransactions
                .OrderByDescending(x => x.Amount)
                .ThenBy(x => x.Timestamp)
                .Take(NumberOfJokerWalletJokerWinners);

            return winningTransactions.Count(x => x.SenderWalletId == walletId);
        }
    }
}