using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("IncomingTransactions")]
    public class IncomingTransactionEntity : Entity
    {
        public Guid Id { get; set; }
        
        public Guid ReceiverWalletId { get; set; }
        public virtual WalletEntity ReceiverWallet { get; set; }
        
        public Guid SenderWalletId { get; set; }
        public int Amount { get; set; }

        public Guid GameId { get; set; }
        public Guid RoundId { get; set; }
        public int RoundNumber { get; set; }
    }
}