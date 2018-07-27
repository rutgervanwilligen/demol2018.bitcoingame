namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        bool WalletIsClosed();
        int GetCurrentBalanceInRound(int currentRoundNumber);
        void WriteTransactions();
    }
}
