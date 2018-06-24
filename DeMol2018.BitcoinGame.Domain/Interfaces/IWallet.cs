using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Domain.Interfaces
{
    public interface IWallet
    {
        bool WalletIsClosed();
        void AddTransaction(Transaction transaction);
        int GetBalance();
        void WriteTransactions();
    }
}
