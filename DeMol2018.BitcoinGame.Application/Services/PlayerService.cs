using System;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Repositories;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class PlayerService
    {
        private PlayerRepository PlayerRepository { get; set; }

        public PlayerService(BitcoinGameDbContext dbContext)
        {
            PlayerRepository = new PlayerRepository(dbContext);
        }

        public Guid? Login(string name, int code)
        {
            var player = PlayerRepository.FindBy(x => x.Name == name
                                         && x.LoginCode == code);

            return player?.Id;
        }
    }
}