using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class JokerWallet : Wallet
    {
        private const int AmountForOneJoker = 1500;
        
        public JokerWallet() : base()
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
            return ReceivedTransactions
                .GroupBy(x => x.SenderId)
                .Select(x => new JokerWinner {
                    PlayerId = x.Key,
                    NumberOfJokersWon = x.Sum(y => y.Amount) / AmountForOneJoker
                });
        }
    }
}
