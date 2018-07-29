using System;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        bool WalletIsClosed();
        int GetCurrentBalanceInGameAndRound(Guid gameId, int roundNumber);
        void WriteTransactions();
    }
}
