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

        public List<IncomingTransaction> IncomingTransactions;
        public List<OutgoingTransaction> OutgoingTransactions;

        protected Wallet()
        {
            Id = new Guid();
            IncomingTransactions = new List<IncomingTransaction>();
            OutgoingTransactions = new List<OutgoingTransaction>();
        }

        public abstract bool WalletIsClosed();

        public int GetCurrentBalanceInRound(int currentRoundNumber)
        {
            // All received amounts excluding the current round
            var receivedAmount = IncomingTransactions
                .Where(x => x.RoundNumber < currentRoundNumber)
                .Sum(x => x.Amount);

            // All sent amounds including the current round
            var sentAmount = OutgoingTransactions
                .Sum(x => x.Amount);

            return StartAmount + receivedAmount - sentAmount;
        }

        public void WriteTransactions()
        {
            throw new NotImplementedException();
        }

        public IncomingTransaction AddIncomingTransaction(
            Guid gameId,
            Guid roundId,
            int roundNumber,
            int amount,
            Guid senderWalletId)
        {
            var incomingTransaction = new IncomingTransaction {
                GameId = gameId,
                RoundId = roundId,
                RoundNumber = roundNumber,
                Amount = amount,
                SenderWalletId = senderWalletId
            };

            IncomingTransactions.Add(incomingTransaction);
            return incomingTransaction;
        }
        
        public OutgoingTransaction MakeTransaction(
                Guid? receiverWalletId,
                int amount,
                Guid currentGameId,
                Guid currentRoundId,
                int currentRoundNumber,
                int? invalidReceiverAddress)
        {
            var currentBalance = GetCurrentBalanceInRound(currentRoundNumber);

            if (amount > currentBalance) {
                throw new InsufficientFundsException("Balance is too low to make transaction.");
            }

            var outgoingTransaction = new OutgoingTransaction {
                Amount = amount,
                ReceiverWalletId = receiverWalletId,
                RoundId = currentRoundId,
                GameId = currentGameId,
                RoundNumber = currentRoundNumber,
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
