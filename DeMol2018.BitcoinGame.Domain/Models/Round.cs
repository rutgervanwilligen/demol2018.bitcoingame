using System;
using System.Collections.Generic;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Round
    {
        public int RoundNumber { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool HasStarted { get; set; }
        public bool HasEnded => HasStarted 
                                && DateTime.UtcNow > EndTime;

        public IEnumerable<Transaction> Transactions { get; set; }
        public int Id { get; set; }

        public Round()
        {
            HasStarted = false;
        }

        public void Start(TimeSpan roundLength)
        {
            StartTime = DateTime.UtcNow;
            EndTime = DateTime.UtcNow.Add(roundLength);
            HasStarted = true;
        }
    }
}
