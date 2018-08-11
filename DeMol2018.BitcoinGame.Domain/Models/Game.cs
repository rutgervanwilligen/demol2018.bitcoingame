using System;
using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public List<Round> Rounds { get; set; }
        public DateTime StartTime { get; set; }
        public bool HasFinished { get; set; }

        public Game()
        {
            Id = new Guid();
            Rounds = new List<Round>();
            StartTime = DateTime.UtcNow;
            HasFinished = false;
        }

        public Round GetCurrentRound()
            => Rounds.Any(x => x.IsActive) ? Rounds.OrderByDescending(x => x.RoundNumber).First() : null;

        public int? GetLastFinishedRoundNumber()
        {
            return Rounds.Any(x => x.HasEnded)
                ? Rounds.Where(x => x.HasEnded).Max(x => x.RoundNumber)
                : (int?) null;
        }
    }
}