using System;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class LargeTransactionWallet : Wallet
    {
        public new readonly int Address = 111;

        private const int MinimalAmountToSendInOneTransaction = 1200;
        private const int EuroPrizeToWinForEachTransaction = 500;
        
        public LargeTransactionWallet()
        {
            StartAmount = 0;
        }

        public override bool WalletIsClosed()
        {
            // This wallet is always open
            return false;
        }

        public int GetEurosWonInGameAndRoundNumber(Guid gameId, int roundNumber)
        {
            var numberOfValidTransactions = IncomingTransactions
                .Count(x => x.GameId == gameId
                         && x.RoundNumber < roundNumber
                         && x.Amount > MinimalAmountToSendInOneTransaction);

            return numberOfValidTransactions * EuroPrizeToWinForEachTransaction;
        }
    }
}
