using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Rounds")]
    public class RoundEntity : Entity
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid GameId { get; set; }
        public virtual GameEntity Game { get; set; }
    }
}