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

            var nonPlayerWalletMoneyWonSoFar = GetNonPlayerWalletMoneyWonUpUntilRound(gameId, roundNumber.Value);

            var playerWalletMoneyWonSoFar = GetPlayerWalletMoneyWonUpUntilRound(gameId, roundNumber.Value);

            return EuroBalanceAtNewGame + nonPlayerWalletMoneyWonSoFar + playerWalletMoneyWonSoFar;
        }

        private int GetPlayerWalletMoneyWonUpUntilRound(Guid gameId, int roundNumber)
        {
            var potentialTransactionsGroupedByRound = WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                         && x.Type == WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel()).ToList()
                .SelectMany(x => x.OutgoingTransactions).ToList()
                .Where(y => y.RoundNumber <= roundNumber
                     && y.ReceiverWalletId.HasValue
                     && y.Amount >= MinimalAmountForStuivertjeWisselen)
                .GroupBy(x => x.RoundNumber)
                .ToList();

            var totalPlayerWalletMoneyWon = 0;

            foreach (var transactions in potentialTransactionsGroupedByRound)
            {
                totalPlayerWalletMoneyWon += MoneySwap.GetMoneySwapResultOfTransactions(transactions.ToList());
            }

            return totalPlayerWalletMoneyWon;
        }

        private int GetNonPlayerWalletMoneyWonUpUntilRound(Guid gameId, int roundNumber)
        {
            return WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                         && x.Type != WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel())
                .ToList()
                .Sum(x => x.GetMoneyWonUpUntilRound(roundNumber));
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