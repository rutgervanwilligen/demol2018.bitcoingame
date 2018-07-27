using System;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class PlayerWallet : Wallet
    {
        public Guid PlayerId { get; set; }
        
        public PlayerWallet()
        {
            StartAmount = 1000;
        }

        public override bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }
    }
}
