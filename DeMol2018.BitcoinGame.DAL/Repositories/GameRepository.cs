using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL.Repositories
{
    public class GameRepository : BitcoinGameBaseRepository<GameEntity>
    {
        public GameRepository(BitcoinGameDbContext context)
            : base(context) { }

        public GameEntity FindCurrentGame()
        {
            return GetAll().SingleOrDefault(x => x.StartTime < DateTime.UtcNow
                                                 && !x.HasFinished);
        }
    }
}
