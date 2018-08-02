using System;
using System.Collections.Generic;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int LoginCode { get; set; }
        public bool IsAdmin { get; set; }
        public int WalletAddress { get; set; }
        
        public IEnumerable<PlayerWallet> Wallets { get; set; }
    }
}
