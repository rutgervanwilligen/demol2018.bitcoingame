using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Domain.Interfaces;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public abstract class Wallet : IWallet
    {
        public Guid Id { get; set; }
        public int Address { get; set; }

        protected readonly List<Transaction> _successfulTransactions;
        protected readonly List<Transaction> _failedTransactions;

        protected Wallet()
        {
            Id = new Guid();
            _successfulTransactions = new List<Transaction>();
            _failedTransactions = new List<Transaction>();
        }

        public abstract bool WalletIsClosed();

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
    }
}
