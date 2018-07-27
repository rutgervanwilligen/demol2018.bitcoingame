using System;
using System.Collections.Generic;
using System.Linq;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Round
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public Guid GameId { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool HasStarted { get; set; }
        public bool HasEnded => HasStarted 
                                && DateTime.UtcNow > EndTime;

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
