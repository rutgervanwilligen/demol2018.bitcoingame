using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class JokerWinner
    {
        public Guid SenderWallet { get; set; }
        public int NumberOfJokersWon { get; set; }
    }
}
