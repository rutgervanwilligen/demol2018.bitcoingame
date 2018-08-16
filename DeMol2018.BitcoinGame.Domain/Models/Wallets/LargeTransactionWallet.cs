using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class LargeTransactionWallet : Wallet
    {
        public new readonly int Address = 111;
        public new readonly string DisplayName = "Solo-transactiewallet";

        private const int MinimalAmountToSendInOneTransaction = 1200;
        private const int EuroPrizeToWinForEachTransaction = 500;
        
        public LargeTransactionWallet()
        {
            StartAmount = 0;
        }

        public override int GetMoneyWonInRound(int roundNumber)
        {
            var numberOfValidTransactions = IncomingTransactions
                .Count(x => x.RoundNumber == roundNumber
                         && x.Amount >= MinimalAmountToSendInOneTransaction);

            return numberOfValidTransactions * EuroPrizeToWinForEachTransaction;
        }
    }
}
