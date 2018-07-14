using System;
using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.DAL.Mappers
{
    public static class WalletMapper
    {
        public static WalletEntity ToEntity(this Wallet wallet)
        {
            throw new NotImplementedException();
//            return new WalletEntity {
//                Id = wallet.Id,
//                SenderId = wallet.Sender.Id,
//                ReceiverId = wallet.Receiver.Id
//            };
        }

        public static Wallet ToDomainModel(this WalletEntity WalletEntity)
        {
            // TODO Switch on type
            throw new NotImplementedException();
//            return new Wallet {
//                Id = WalletEntity.Id,
//                Amount = WalletEntity.Amount,
//                Sender = WalletEntity.SenderWallet.ToDomainModel(),
//                Receiver = WalletEntity.ReceiverWallet.ToDomainModel()
//            };
        }
    }
}