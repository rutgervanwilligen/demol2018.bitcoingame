using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Games")]
    public class GameEntity : Entity
    {
        public Guid Id { get; set; }
        
        public DateTime StartTime { get; set; }
        public bool HasFinished { get; set; }
        public bool IsCurrentGame { get; set; }
 
        public virtual ICollection<RoundEntity> Rounds { get; set; }
        public virtual ICollection<WalletEntity> Wallets { get; set; }
    }
}