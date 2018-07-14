using System.Collections.Generic;
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
                StartTime = round.StartTime
            };
        }

        public static Round ToDomainModel(this RoundEntity roundEntity)
        {
            return new Round {
                Id = roundEntity.Id,
                StartTime = roundEntity.StartTime,
                Transactions = roundEntity.Transactions.Select(x => x.ToDomainModel())
            };
        }
    }
}