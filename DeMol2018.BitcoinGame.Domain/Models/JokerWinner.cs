﻿using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class JokerWinner
    {
        public Guid PlayerId { get; set; }
        public int NumberOfJokersWon { get; set; }
    }
}
