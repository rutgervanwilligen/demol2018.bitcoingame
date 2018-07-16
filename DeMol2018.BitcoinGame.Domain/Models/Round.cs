using System;
using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Round
    {
        public int Id { get; set; }
        public int RoundNumber { get; set; }
        public Game Game { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool HasStarted { get; set; }
        public bool HasEnded => HasStarted 
                                && DateTime.UtcNow > EndTime;

        public IEnumerable<Transaction> Transactions { get; set; }

        public Round()
        {
            HasStarted = false;
            Transactions = Enumerable.Empty<Transaction>();
        }

        public void Start(TimeSpan roundLength)
        {
            StartTime = DateTime.UtcNow;
            EndTime = DateTime.UtcNow.Add(roundLength);
            HasStarted = true;
        }
    }
}
