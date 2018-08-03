using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class TransactionMapper
    {
        public static IncomingTransactionEntity ToEntity(this IncomingTransaction transaction)
        {
            return new IncomingTransactionEntity {
                Id = transaction.Id,
//                ReceiverWalletId = transaction.ReceiverWalletId,
                SenderWalletId = transaction.SenderWalletId,
                RoundNumber = transaction.RoundNumber,
                Amount = transaction.Amount
            };
        }

        public static IncomingTransaction ToDomainModel(this IncomingTransactionEntity transactionEntity)
        {
            return new IncomingTransaction {
                Id = transactionEntity.Id,
                Amount = transactionEntity.Amount,
//                ReceiverWalletId = transactionEntity.ReceiverWalletId,
                SenderWalletId = transactionEntity.SenderWalletId,
                RoundNumber = transactionEntity.RoundNumber
            };
        }
        
        public static OutgoingTransactionEntity ToEntity(this OutgoingTransaction transaction)
        {
            return new OutgoingTransactionEntity {
                Id = transaction.Id,
                SenderWalletId = transaction.SenderWalletId,
                ReceiverWalletId = transaction.ReceiverWalletId,
                RoundNumber = transaction.RoundNumber,
                Amount = transaction.Amount,
                InvalidReceiverAddress = transaction.InvalidReceiverAddress
            };
        }

        public static OutgoingTransaction ToDomainModel(this OutgoingTransactionEntity transactionEntity)
        {
            return new OutgoingTransaction {
                Id = transactionEntity.Id,
                Amount = transactionEntity.Amount,
                InvalidReceiverAddress = transactionEntity.InvalidReceiverAddress,
                SenderWalletId = transactionEntity.SenderWalletId,
                ReceiverWalletId = transactionEntity.ReceiverWalletId,
                RoundNumber = transactionEntity.RoundNumber
            };
        }
    }
}