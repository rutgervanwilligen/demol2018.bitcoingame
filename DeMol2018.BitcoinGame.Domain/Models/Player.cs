using System;

namespace DeMol2018.BitcoinGame.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginCode { get; set; }
    }
}
