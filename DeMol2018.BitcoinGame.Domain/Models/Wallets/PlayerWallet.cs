using System;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class PlayerWallet : Wallet
    {
        public Guid PlayerId { get; set; }
        public new readonly string DisplayName = "Mijn wallet";

        public PlayerWallet()
        {
            StartAmount = 1000;
        }

        public override int GetMoneyWonInRound(int roundNumber)
        {
            return 0;
        }

        public override bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }
    }
}
