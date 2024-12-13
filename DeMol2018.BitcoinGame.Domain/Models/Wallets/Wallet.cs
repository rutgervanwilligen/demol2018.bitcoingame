﻿using System;
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
        public Guid GameId { get; set; }
        public string DisplayName { get; }

        public List<IncomingTransaction> IncomingTransactions;
        public List<OutgoingTransaction> OutgoingTransactions;

        protected Wallet()
        {
            Id = Guid.NewGuid();
            IncomingTransactions = new List<IncomingTransaction>();
            OutgoingTransactions = new List<OutgoingTransaction>();
        }

        public abstract int GetMoneyWonInRound(int roundNumber);

        public int GetBalanceAfterRound(int roundNumber)
        {
            // All received amounts up until this round
            var receivedAmount = IncomingTransactions
                .Where(x => x.RoundNumber <= roundNumber)
                .Sum(x => x.Amount);

            // All sent amounds including non-finished round
            var sentAmount = OutgoingTransactions
                .Sum(x => x.Amount);

            return StartAmount + receivedAmount - sentAmount;
        }

        public int GetFinalBalance()
        {
            var receivedAmount = IncomingTransactions.Sum(x => x.Amount);
            var sentAmount = OutgoingTransactions.Sum(x => x.Amount);

            return StartAmount + receivedAmount - sentAmount;
        }

        public IncomingTransaction AddIncomingTransaction(
            int roundNumber,
            int amount,
            Guid senderWalletId)
        {
            var incomingTransaction = new IncomingTransaction {
                RoundNumber = roundNumber,
                Amount = amount,
                SenderWalletId = senderWalletId,
                Timestamp = DateTime.UtcNow
            };

            IncomingTransactions.Add(incomingTransaction);
            return incomingTransaction;
        }
        
        public OutgoingTransaction MakeTransaction(
                Guid? receiverWalletId,
                int amount,
                int roundNumber,
                int? invalidReceiverAddress)
        {
            var currentBalance = GetBalanceAfterRound(roundNumber);

            if (amount > currentBalance) {
                throw new InsufficientFundsException("Balance is too low to make transaction.");
            }

            var outgoingTransaction = new OutgoingTransaction {
                Amount = amount,
                ReceiverWalletId = receiverWalletId,
                RoundNumber = roundNumber,
                Timestamp = DateTime.UtcNow,
                InvalidReceiverAddress = receiverWalletId == null
                    ? invalidReceiverAddress
                    : null
            };

            OutgoingTransactions.Add(outgoingTransaction);
            return outgoingTransaction;
        }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message)
        {            
        }
    }
}
