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
        public int StartAmount { get; set; }

        public List<Transaction> ReceivedTransactions;
        public List<Transaction> SentTransactions;

        protected Wallet()
        {
            Id = new Guid();
            ReceivedTransactions = new List<Transaction>();
            SentTransactions = new List<Transaction>();
        }

        public abstract bool WalletIsClosed();

        public void AddTransaction(Transaction transaction)
        {
            if (!WalletIsClosed())
            {
                ReceivedTransactions.Add(transaction);
            }
            else
            {
                SentTransactions.Add(transaction);
            }
        }

        public int GetBalance()
        {
            var receivedAmount = ReceivedTransactions.Sum(x => x.Amount);
            var sentAmount = SentTransactions.Sum(x => x.Amount);

            return StartAmount + receivedAmount - sentAmount;
        }

        public void WriteTransactions()
        {
            throw new System.NotImplementedException();
        }
    }
}
