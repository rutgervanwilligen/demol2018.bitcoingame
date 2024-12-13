﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Wallets")]
    public class WalletEntity : Entity
    {
        public Guid Id { get; set; }

        public int Address { get; set; }
        public string Type { get; set; }

        public int StartAmount { get; set; }

        public virtual ICollection<OutgoingTransactionEntity> OutgoingTransactions { get; set; }
        public virtual ICollection<IncomingTransactionEntity> IncomingTransactions { get; set; }

        public Guid? PlayerId { get; set; }
        public virtual PlayerEntity Player { get; set; }

        public Guid GameId { get; set; }
        public virtual GameEntity Game { get; set; }

        public enum WalletType
        {
            PlayerWallet = 0,
            JokerWallet = 1,
            CombinedTransactionWallet = 2,
            LargeTransactionWallet = 3
        }
    }
}