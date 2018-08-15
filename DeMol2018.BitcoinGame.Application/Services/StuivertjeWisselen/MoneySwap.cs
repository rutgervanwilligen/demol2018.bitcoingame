using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen
{
    public class MoneySwap
    {
        public static int GetMoneySwapResultOfTransactions(List<OutgoingTransaction> transactions)
        {
            var results = new HashSet<SearchResult>();

            foreach (var transaction in transactions)
            {
                foreach (var newResult in MoneySwapSearch.Search(transactions, transaction))
                {
                    results.Add(newResult);
                }
            }

            var sortedResults = results.OrderByDescending(x => x.Depth);
            var finalResults = new HashSet<SearchResult>();
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
    }
}