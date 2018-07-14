using System;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class TransactionEntityMapper
    {
        public static TransactionEntity ToEntity(this Transaction transaction)
        {
            throw new NotImplementedException();
//            return new TransactionEntity {
//                Id = transaction.Id,
//                SenderId = transaction.Sender.Id,
//                ReceiverId = transaction.Receiver.Id
 //           };
        }

        public static Transaction ToDomainModel(this TransactionEntity transactionEntity)
        {
            return new Transaction {
                Id = transactionEntity.Id,
                Amount = transactionEntity.Amount,
                Sender = transactionEntity.SenderWallet.ToDomainModel(),
                Receiver = transactionEntity.ReceiverWallet.ToDomainModel()
            };
        }
    }
}