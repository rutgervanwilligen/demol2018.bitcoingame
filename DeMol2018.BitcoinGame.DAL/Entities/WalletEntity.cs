using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Wallets")]
    public class WalletEntity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public int Address { get; set; }
        public WalletType Type { get; set; }
        
        public int StartAmount { get; set; }
        
        public virtual ICollection<TransactionEntity> SentTransactions { get; set; }
        public virtual ICollection<TransactionEntity> ReceivedTransactions { get; set; }

        public Guid? PlayerId { get; set; }
        public virtual PlayerEntity Player { get; set; }
        
        public enum WalletType
        {
            JokerWallet,
            PlayerWallet
        }
    }
}