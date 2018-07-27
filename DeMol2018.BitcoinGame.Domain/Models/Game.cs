﻿using System;
using System.Collections.Generic;

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
    }
}