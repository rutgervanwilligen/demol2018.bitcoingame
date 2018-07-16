using System;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class PlayerService
    {
        private PlayerRepository PlayerRepository { get; set; }

        public PlayerService(BitcoinGameDbContext dbContext)
        {
            PlayerRepository = new PlayerRepository(dbContext);
        }

        public Player Login(string name, int code)
        {
            return PlayerRepository.FindBy(x => x.Name == name
                                         && x.LoginCode == code).ToDomainModel();
        }

        public Player GetById(Guid invokerId)
        {
            return PlayerRepository.GetBy(x => x.Id == invokerId).ToDomainModel();
        }
    }
}