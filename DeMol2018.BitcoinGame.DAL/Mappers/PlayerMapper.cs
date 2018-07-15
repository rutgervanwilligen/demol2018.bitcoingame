using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerEntity ToEntity(this Player player)
        {
            return new PlayerEntity {
                Id = player.Id,
                LoginCode = player.LoginCode,
                Name = player.Name
            };
        }

        public static Player ToDomainModel(this PlayerEntity player)
        {
            return new Player {
                Id = player.Id,
                LoginCode = player.LoginCode,
                Name = player.Name
            };
        }
    }
}