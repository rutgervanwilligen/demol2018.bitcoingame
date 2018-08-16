using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class CombinedTransactionWallet : Wallet
    {
        public new readonly int Address = 222;
        public new readonly string DisplayName = "Groeps-transactiewallet";

        private const int MinimalNumberOfCandidatesToWin = 3;
        private const int MinimalCombinedTransactionAmountToWin = 1200;
        private const int EuroPrizeToWinPerRound = 250;
        
        public CombinedTransactionWallet()
        {
            StartAmount = 0;
        }

        public override int GetMoneyWonInRound(int roundNumber)
        {
            var transactionsInRound = IncomingTransactions
                .Where(x => x.RoundNumber == roundNumber)
                .ToList();

            if (transactionsInRound.Count < MinimalNumberOfCandidatesToWin
                || transactionsInRound.Sum(x => x.Amount) <= MinimalCombinedTransactionAmountToWin)
            {
                return 0;
            }

            return EuroPrizeToWinPerRound;
        }
    }
}
