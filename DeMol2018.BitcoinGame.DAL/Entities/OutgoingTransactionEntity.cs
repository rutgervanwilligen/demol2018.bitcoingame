﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("OutgoingTransactions")]
    public class OutgoingTransactionEntity : Entity
    {
        public Guid Id { get; set; }
        
        public Guid SenderWalletId { get; set; }
        public virtual WalletEntity SenderWallet { get; set; }
        
        public Guid? ReceiverWalletId { get; set; }
        public int Amount { get; set; }

        public Guid GameId { get; set; }
        public Guid RoundId { get; set; }
        public int RoundNumber { get; set; }

        public int? InvalidReceiverAddress { get; set; }
    }
}