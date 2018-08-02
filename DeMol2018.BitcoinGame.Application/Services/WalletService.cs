using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class WalletService
    {
        private WalletRepository WalletRepository { get; set; }

        public WalletService(BitcoinGameDbContext dbContext)
        {
            WalletRepository = new WalletRepository(dbContext);
        }

        public Wallet GetWalletByGameIdAndPlayerId(Guid gameId, Guid playerId)
        {
            return WalletRepository
                .GetBy(x => x.PlayerId == playerId
                         && x.GameId == gameId)
                .ToDomainModel();
        }

        public List<Wallet> GetNonPlayerWalletsByGameId(Guid gameId)
        {
            return WalletRepository
                .GetAll()
                .Where(x => x.GameId == gameId
                            && x.Type != WalletEntity.WalletType.PlayerWallet.ToString())
                .Select(x => x.ToDomainModel())
                .ToList();
        }
    }
}