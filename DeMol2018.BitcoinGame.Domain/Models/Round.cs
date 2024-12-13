﻿using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Round
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public Guid GameId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool HasStarted => true;
        public bool HasEnded => DateTime.UtcNow > EndTime;

        public bool IsActive => HasStarted && !HasEnded;

        public Round()
        {
            Id = Guid.NewGuid();
        }

        public void Start(TimeSpan roundLength)
        {
            StartTime = DateTime.UtcNow;
            EndTime = DateTime.UtcNow.Add(roundLength);
        }
    }
}
