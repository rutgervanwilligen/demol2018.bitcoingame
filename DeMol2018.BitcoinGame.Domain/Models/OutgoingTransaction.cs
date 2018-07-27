﻿using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class OutgoingTransaction
    {
        public Guid Id { get; set; }
        public Guid SenderWalletId { get; set; }
        public Guid? ReceiverWalletId { get; set; }
        public int Amount { get; set; }

        public Guid GameId { get; set; }
        public Guid RoundId { get; set; }
        public int RoundNumber { get; set; }

        public int? InvalidReceiverAddress { get; set; }
    }
}