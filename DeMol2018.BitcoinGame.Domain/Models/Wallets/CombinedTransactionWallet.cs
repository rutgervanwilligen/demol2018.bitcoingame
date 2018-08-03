using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class CombinedTransactionWallet : Wallet
    {
        public new readonly int Address = 222;
        public new readonly string DisplayName = "Combinatie-transactiewallet";

        private const int MinimalNumberOfCandidatesToWin = 3;
        private const int MinimalCombinedTransactionAmountToWin = 1200;
        private const int EuroPrizeToWinPerRound = 250;
        
        public CombinedTransactionWallet()
        {
            StartAmount = 0;
        }

        public override bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }

        public override int GetMoneyWonInRound(int roundNumber)
        {
            var transactionsGroupedByRound = IncomingTransactions
                .GroupBy(x => x.RoundNumber)
                .Where(x => x.Key < roundNumber)
                .Where(x => x.Count() >= MinimalNumberOfCandidatesToWin);

            var numberOfValidCombinedTransactions = transactionsGroupedByRound
                .Count(x => x.Sum(y => y.Amount) >= MinimalCombinedTransactionAmountToWin);

            return numberOfValidCombinedTransactions * EuroPrizeToWinPerRound;
        }
    }
}
