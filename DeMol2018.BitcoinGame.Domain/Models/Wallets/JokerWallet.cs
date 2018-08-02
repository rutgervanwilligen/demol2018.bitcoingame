using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class JokerWallet : Wallet
    {
        public new readonly int Address = 666;
        public new readonly string DisplayName = "Jokerwallet";

        private const int AmountForOneJoker = 1500;
        
        public JokerWallet()
        {
            StartAmount = 0;
        }

        public override bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }

        public IEnumerable<JokerWinner> GetJokerWinners()
        {
            return IncomingTransactions
                .GroupBy(x => x.SenderWalletId)
                .Select(x => new JokerWinner {
                    SenderWallet = x.Key,
                    NumberOfJokersWon = x.Sum(y => y.Amount) / AmountForOneJoker
                });
        }
    }
}
