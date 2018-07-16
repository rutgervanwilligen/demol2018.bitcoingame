using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class RoundEntityMapper
    {
        public static RoundEntity ToEntity(this Round round)
        {
            return new RoundEntity {
                Id = round.Id,
                StartTime = round.StartTime,
                GameId = round.Game.Id,
                EndTime = round.EndTime,
                RoundNumber = round.RoundNumber
            };
        }

        public static Round ToDomainModel(this RoundEntity roundEntity)
        {
            return new Round {
                Id = roundEntity.Id,
                StartTime = roundEntity.StartTime,
                RoundNumber = roundEntity.RoundNumber,
                Transactions = roundEntity.Transactions == null
                    ? Enumerable.Empty<Transaction>()
                    : roundEntity.Transactions.Select(x => x.ToDomainModel())
            };
        }
    }
}