using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Wallets")]
    public class WalletEntity
    {
        public int Id { get; set; }

        public virtual ICollection<TransactionEntity> SentTransactions { get; set; }
        public virtual ICollection<TransactionEntity> ReceivedTransactions { get; set; }

        public Guid PlayerId { get; set; }
        public virtual PlayerEntity Player { get; set; }
    }
}