using System;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        int GetMoneyWonUntilRound(int roundNumber);
        bool WalletIsClosed();
        int GetCurrentBalanceAfterRound(int roundNumber);
        void WriteTransactions();
    }
}
