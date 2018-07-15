using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Transactions")]
    public class TransactionEntity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public int Amount { get; set; }

        public int RoundId { get; set; }
        public virtual RoundEntity Round { get; set; }

        public Guid SenderId { get; set; }
        public virtual WalletEntity SenderWallet { get; set; }

        public Guid ReceiverId { get; set; }
        public virtual WalletEntity ReceiverWallet { get; set; }
    }
}