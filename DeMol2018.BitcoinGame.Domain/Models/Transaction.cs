using System;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Wallet Sender { get; set; }
        public Wallet Receiver { get; set; }
        public int Amount { get; set; }
    }
}
