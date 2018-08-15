using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen
{
    public class MoneySwapSearch
    {
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

        private class TransactionDepthTuple
        {
            public int Depth { get; set; }
            public OutgoingTransaction Transaction { get; set; }
            public HashSet<Guid> Traversal { get; set; }
        }
    }
}