using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class RoundMapper
    {
        public static RoundEntity ToEntity(this Round round)
        {
            return new RoundEntity {
                Id = round.Id,
                GameId = round.GameId,
                StartTime = round.StartTime,
                EndTime = round.EndTime,
                RoundNumber = round.RoundNumber
            };
        }

        public static Round ToDomainModel(this RoundEntity roundEntity)
        {
            return new Round {
                Id = roundEntity.Id,
                GameId = roundEntity.GameId,
                StartTime = roundEntity.StartTime,
                EndTime = roundEntity.EndTime,
                RoundNumber = roundEntity.RoundNumber,
                Transactions = roundEntity.Transactions == null
                    ? new List<Transaction>()
                    : roundEntity.Transactions.Select(x => x.ToDomainModel()).ToList()
            };
        }
    }
}