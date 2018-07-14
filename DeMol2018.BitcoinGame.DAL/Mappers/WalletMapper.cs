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
                    return (JokerWallet) null;
                case WalletEntity.WalletType.PlayerWallet:
                    return (PlayerWallet) null;
                default:
                    throw new Exception("Unexpected wallet type in database");
            }
            
//            return new Wallet {
//                Id = walletEntity.Id,
//                Amount = walletEntity.Amount,
//                Sender = walletEntity.SenderWallet.ToDomainModel(),
//                Receiver = walletEntity.ReceiverWallet.ToDomainModel()
//            };
        }
    }
}