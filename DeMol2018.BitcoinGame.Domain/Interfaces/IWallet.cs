using System;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        int GetMoneyWonInRound(int roundNumber);
        int GetBalanceAfterRound(int roundNumber);
        int GetFinalBalance();
    }
}
