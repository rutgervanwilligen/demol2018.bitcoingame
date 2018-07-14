using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class GameEntityMapper
    {
        public static GameEntity ToEntity(this Game game)
        {
            return new GameEntity {
                Id = game.Id,
                StartTime = game.StartTime,
                HasFinished = game.HasFinished
            };
        }

        public static Game ToDomainModel(this GameEntity gameEntity)
        {
            return new Game {
                Id = gameEntity.Id,
                StartTime = gameEntity.StartTime,
                HasFinished = gameEntity.HasFinished,
                Rounds = gameEntity.Rounds.Select(x => x.ToDomainModel())
            };
        }
    }
}