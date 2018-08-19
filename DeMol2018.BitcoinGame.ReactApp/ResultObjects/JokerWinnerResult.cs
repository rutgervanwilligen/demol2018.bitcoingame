using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.ReactApp.ResultObjects
{
    public class JokerWinnerResult
    {
        public string Name { get; set; }
        public int NumberOfJokersWon { get; set; }

        public static JokerWinnerResult MapFrom(JokerWinner jokerWinner)
        {
            return new JokerWinnerResult {
                Name = jokerWinner.PlayerName,
                NumberOfJokersWon = jokerWinner.NumberOfJokersWon
            };
        }
    }
}