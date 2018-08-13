using System;
using System.Collections.Generic;
using System.Linq;
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
            // Stuivertje wisselen
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

            var results = new HashSet<MoneySwapSearch.SearchResult>();

            foreach (var transactionsPerRound in potentialTransactionsGroupedByRound)
            {
                foreach (var transaction in transactionsPerRound)
                {
                    foreach (var newResult in MoneySwapSearch.Search(transactionsPerRound.ToList(), transaction))
                    {
                        results.Add(newResult);
                    }
                }
            }

            var sortedResults = results.OrderByDescending(x => x.Depth);
            var finalResults = new HashSet<MoneySwapSearch.SearchResult>();
            var seenTransactionIds = new HashSet<Guid>();

            foreach (var result in sortedResults)
            {
                var valid = true;
                foreach (var transactionId in result.TransactionGuids)
                {
                    if (seenTransactionIds.Contains(transactionId))
                    {
                        valid = false;
                    }
                }

                if (!valid)
                {
                    continue;
                }

                finalResults.Add(result);
                foreach (var transactionId in result.TransactionGuids)
                {
                    seenTransactionIds.Add(transactionId);
                }
            }

            return finalResults.Sum(x => (x.Depth - 1) * 200);
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

        public static class MoneySwapSearch
        {
            private class TransactionDepthTuple
            {
                public int Depth { get; set; }
                public OutgoingTransaction Transaction { get; set; }
                public HashSet<Guid> Traversal { get; set; }
            }

            public class SearchResult : IEquatable<SearchResult>
            {
                public int Depth => TransactionGuids.Count;
                public HashSet<Guid> TransactionGuids { get; set; }

                public override int GetHashCode()
                {
                    return Depth;
                }

                public bool Equals(SearchResult other)
                {
                    return other != null && TransactionGuids.SetEquals(other.TransactionGuids);
                }
            }

            public static HashSet<SearchResult> Search(List<OutgoingTransaction> transactions, OutgoingTransaction root)
            {
                var results = new HashSet<SearchResult>();
                var queue = new Queue<TransactionDepthTuple>();
                var seenTransactionIds = new HashSet<Guid>();

                queue.Enqueue(new TransactionDepthTuple {
                    Transaction = root,
                    Depth = 0,
                    Traversal = new HashSet<Guid>{root.Id}
                });

                seenTransactionIds.Add(root.Id);

                while (queue.Count > 0)
                {
                    var tuple = queue.Dequeue();
                    if (tuple.Transaction.ReceiverWalletId == root.SenderWalletId)
                    {
                        results.Add(new SearchResult {
                            TransactionGuids = tuple.Traversal
                        });
                        continue;
                    }

                    var receiverId = tuple.Transaction.ReceiverWalletId;

                    if (receiverId == null)
                    {
                        continue;
                    }

                    var newTransactions = transactions.Where(x => x.SenderWalletId == receiverId.Value
                                                                  && !seenTransactionIds.Contains(x.Id));

                    foreach (var newTransaction in newTransactions)
                    {
                        seenTransactionIds.Add(newTransaction.Id);
                        queue.Enqueue(new TransactionDepthTuple {
                            Depth = tuple.Depth + 1,
                            Transaction = newTransaction,
                            Traversal = new HashSet<Guid>(tuple.Traversal) {newTransaction.Id}
                        });
                    }
                }

                return results;
            }
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