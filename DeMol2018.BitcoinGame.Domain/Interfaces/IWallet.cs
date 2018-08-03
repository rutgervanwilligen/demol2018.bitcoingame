using System;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        int GetMoneyWonInRound(int roundNumber);
        bool WalletIsClosed();
        int GetCurrentBalanceInRound(int roundNumber);
        void WriteTransactions();
    }
}
