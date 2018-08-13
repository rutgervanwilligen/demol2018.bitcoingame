using System;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        int GetMoneyWonUpUntilRound(int roundNumber);
        int GetFinalBalance();
        int GetBalanceAfterRound(int roundNumber);
    }
}
