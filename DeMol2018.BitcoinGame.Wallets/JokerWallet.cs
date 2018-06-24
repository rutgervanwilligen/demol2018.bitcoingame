using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Domain.Interfaces;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Wallets
{
    public class JokerWallet : IWallet
    {
        private const int AmountForOneJoker = 1500;
        private Guid WalletIdentifier;

        private readonly List<Transaction> _successfulTransactions;
        private readonly List<Transaction> _failedTransactions;

        public JokerWallet()
        {
            WalletIdentifier = new Guid();
            _successfulTransactions = new List<Transaction>();
            _failedTransactions = new List<Transaction>();
        }

        public bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (!WalletIsClosed())
            {
                _successfulTransactions.Add(transaction);
            }
            else
            {
                _failedTransactions.Add(transaction);
            }
        }

        public int GetBalance()
        {
            return _successfulTransactions.Sum(x => x.Amount);
        }

        public void WriteTransactions()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<JokerWinner> GetJokerWinners()
        {
            return _successfulTransactions
                .GroupBy(x => x.Sender)
                .Select(x => new JokerWinner {
                    Player = x.Key,
                    NumberOfJokersWon = x.Sum(y => y.Amount) / AmountForOneJoker
                });
        }
    }
}
