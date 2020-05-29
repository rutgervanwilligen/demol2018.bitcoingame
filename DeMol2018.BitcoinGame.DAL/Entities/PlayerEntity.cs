using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeMol2018.BitcoinGame.DAL.Entities
{
    [Table("Players")]
    public class PlayerEntity : Entity
    {
        public Guid Id { get; set; }

        public int LoginCode { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public int WalletAddress { get; set; }

        public virtual ICollection<WalletEntity> Wallets { get; set; }
    }
}
