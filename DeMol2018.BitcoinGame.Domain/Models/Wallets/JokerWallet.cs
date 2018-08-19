using System;
using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models.Wallets
{
    public class JokerWallet : Wallet
    {
        public new readonly int Address = 666;
        public new readonly string DisplayName = "Jokerwallet";

        private const int NumberOfJokersGiven = 8;
        
        public JokerWallet()
        {
            StartAmount = 0;
        }

        public override int GetMoneyWonInRound(int roundNumber)
        {
            return 0;
        }

        public IEnumerable<JokerWalletJokerWinner> GetJokerWinners()
        {
            return IncomingTransactions
                .OrderByDescending(x => x.Amount)
                .ThenBy(x => x.Timestamp)
                .Take(NumberOfJokersGiven)
                .GroupBy(x => x.SenderWalletId)
                .Select(x => new JokerWalletJokerWinner {
                    SenderWalletId = x.Key,
                    NumberOfJokersWon = x.Count()
                });
        }

        public class JokerWalletJokerWinner
        {
            public Guid SenderWalletId { get; set; }
            public int NumberOfJokersWon { get; set; }
        }
    }
}
