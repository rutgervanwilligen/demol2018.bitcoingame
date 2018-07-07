using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Games")]
    public class GameEntity
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
 
        public virtual ICollection<RoundEntity> Rounds { get; set; }
        
    }
}