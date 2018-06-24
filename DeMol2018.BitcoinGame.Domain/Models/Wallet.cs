namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Transaction
    {
        public Player Sender { get; set; }
        public int Amount { get; set; }
    }
}
