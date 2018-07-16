using System;
using System.Linq;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class WalletMapper
    {
        public static WalletEntity ToEntity(this Wallet wallet)
        {
            switch (wallet)
            {
                case PlayerWallet playerWallet:
                    return new WalletEntity {
                        Id = playerWallet.Id,
                        Address = playerWallet.Address,
                        StartAmount = playerWallet.StartAmount,
                        PlayerId = playerWallet.Player.Id,
                        Type = WalletEntity.WalletType.PlayerWallet
                        // TODO Transactions
                    };
                case JokerWallet jokerWallet:
                    return new WalletEntity {
                        Id = jokerWallet.Id,
                        Address = jokerWallet.Address,
                        StartAmount = jokerWallet.StartAmount,
                        PlayerId = null,
                        Type = WalletEntity.WalletType.JokerWallet
                        // TODO Transactions
                    };
            }

            throw new Exception("Unable to map wallet to entity");
        }

        public static Wallet ToDomainModel(this WalletEntity walletEntity)
        {
            switch (walletEntity.Type)
            {
                case WalletEntity.WalletType.JokerWallet:
                    return new JokerWallet {
                        Id = walletEntity.Id,
                        Address = walletEntity.Address,
                        StartAmount = walletEntity.StartAmount,
                        ReceivedTransactions = walletEntity.ReceivedTransactions?.Select(x => x.ToDomainModel()).ToList() ?? Enumerable.Empty<Transaction>().ToList()
                    };
                case WalletEntity.WalletType.PlayerWallet:
                    return new PlayerWallet {
                        Id = walletEntity.Id,
                        Address = walletEntity.Address,
                        StartAmount = walletEntity.StartAmount,
                        ReceivedTransactions = walletEntity.ReceivedTransactions?.Select(x => x.ToDomainModel()).ToList() ?? Enumerable.Empty<Transaction>().ToList(),
                        SentTransactions = walletEntity.SentTransactions?.Select(x => x.ToDomainModel()).ToList() ?? Enumerable.Empty<Transaction>().ToList()
                    };
                default:
                    throw new Exception("Unexpected wallet type in database");
            }
        }
    }
}