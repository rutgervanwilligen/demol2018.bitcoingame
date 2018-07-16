using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerEntity ToEntity(this Player player)
        {
            return new PlayerEntity {
                Id = player.Id,
                LoginCode = player.LoginCode,
                Name = player.Name,
                IsAdmin = player.IsAdmin
            };
        }

        public static Player ToDomainModel(this PlayerEntity player)
        {
            return new Player {
                Id = player.Id,
                LoginCode = player.LoginCode,
                Name = player.Name,
                IsAdmin = player.IsAdmin,
                Wallets = player.Wallets.Select(x => (PlayerWallet)x.ToDomainModel())
            };
        }
    }
}