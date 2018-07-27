using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionEntity ToEntity(this Transaction transaction)
        {
            return new TransactionEntity {
                SenderId = transaction.SenderId,
                ReceiverId = transaction.ReceiverId,
                RoundId = transaction.RoundId,
                RoundNumber = transaction.RoundNumber,
                Amount = transaction.Amount,
                InvalidReceiverAddress = transaction.InvalidReceiverAddress
            };
        }

        public static Transaction ToDomainModel(this TransactionEntity transactionEntity)
        {
            return new Transaction {
                Id = transactionEntity.Id,
                Amount = transactionEntity.Amount,
                InvalidReceiverAddress = transactionEntity.InvalidReceiverAddress,
                SenderId = transactionEntity.SenderId,
                ReceiverId = transactionEntity.ReceiverId,
                RoundId = transactionEntity.RoundId,
                RoundNumber = transactionEntity.RoundNumber
            };
        }
    }
}