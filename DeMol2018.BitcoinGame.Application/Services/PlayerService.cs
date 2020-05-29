using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class PlayerService
    {
        private PlayerRepository PlayerRepository { get; set; }
        private WalletRepository WalletRepository { get; set; }

        public PlayerService(BitcoinGameDbContext dbContext)
        {
            PlayerRepository = new PlayerRepository(dbContext);
            WalletRepository = new WalletRepository(dbContext);
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

        public void CreateNewWalletsForGame(Guid gameId)
        {
            var players = PlayerRepository
                .GetAll()
                .Where(x => !x.IsAdmin)
                .ToList();

            foreach (var player in players)
            {
                var newWallet = new PlayerWallet {
                    GameId = gameId,
                    Address = player.WalletAddress,
                    IncomingTransactions = new List<IncomingTransaction>(),
                    OutgoingTransactions = new List<OutgoingTransaction>()
                };

                player.Wallets.Add(newWallet.ToEntity());
            }

            PlayerRepository.SaveChanges();

            WalletRepository.Add(new JokerWallet {
                GameId = gameId
            }.ToEntity());

            WalletRepository.Add(new CombinedTransactionWallet {
                GameId = gameId
            }.ToEntity());

            WalletRepository.Add(new LargeTransactionWallet {
                GameId = gameId
            }.ToEntity());

            WalletRepository.SaveChanges();
        }
    }
}