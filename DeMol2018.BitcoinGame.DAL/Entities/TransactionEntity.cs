using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Transactions")]
    public class TransactionEntity
    {
        public Guid Id { get; set; }
        public int Value { get; set; }

        public int RoundId { get; set; }
        public virtual RoundEntity Round { get; set; }

        public int SenderId { get; set; }
        public virtual WalletEntity SenderWallet { get; set; }

        public int ReceiverId { get; set; }
        public virtual WalletEntity ReceiverWallet { get; set; }
    }
}