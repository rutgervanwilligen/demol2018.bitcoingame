using System;
using DeMol2018.BitcoinGame.DAL.Entities;
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
                        PlayerId = playerWallet.Player.Id,
                        Type = WalletEntity.WalletType.PlayerWallet
                        // TODO Transactions
                    };
                case JokerWallet jokerWallet:
                    return new WalletEntity {
                        Id = jokerWallet.Id,
                        Address = jokerWallet.Address,
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
                        Address = walletEntity.Address
                    };
                case WalletEntity.WalletType.PlayerWallet:
                    return new PlayerWallet {
                        Id = walletEntity.Id,
                        Address = walletEntity.Address,
                        Player = walletEntity.Player.ToDomainModel()
                    };
                default:
                    throw new Exception("Unexpected wallet type in database");
            }
        }
    }
}