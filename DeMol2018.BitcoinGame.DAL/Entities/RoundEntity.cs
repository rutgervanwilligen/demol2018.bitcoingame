using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Rounds")]
    public class RoundEntity : Entity
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<TransactionEntity> Transactions { get; set; }
        
        public Guid GameId { get; set; }
        public virtual GameEntity Game { get; set; }
    }
}