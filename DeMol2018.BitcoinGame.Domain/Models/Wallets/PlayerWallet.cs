namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class PlayerWallet : Wallet
    {
        public Player Player { get; set; }
        
        public PlayerWallet() : base()
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
