using System;
using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public IEnumerable<Round> Rounds { get; set; }
        public DateTime StartTime { get; set; }
        public bool HasFinished { get; set; }

        public Game()
        {
            Id = new Guid();
            Rounds = Enumerable.Empty<Round>();
            StartTime = DateTime.UtcNow;
            HasFinished = false;
        }
        
    }
}