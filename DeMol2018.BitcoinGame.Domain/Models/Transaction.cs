using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public int Amount { get; set; }

        public Guid RoundId { get; set; }
        public int RoundNumber { get; set; }

        public int? InvalidReceiverAddress { get; set; }
    }
}
