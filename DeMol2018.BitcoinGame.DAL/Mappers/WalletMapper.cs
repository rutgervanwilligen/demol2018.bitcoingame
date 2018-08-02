using System;
using System.Collections.Generic;
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
                        GameId = playerWallet.GameId,
                        Address = playerWallet.Address,
                        StartAmount = playerWallet.StartAmount,
                        PlayerId = playerWallet.PlayerId,
                        Type = WalletEntity.WalletType.PlayerWallet.ToString(),
                        OutgoingTransactions = playerWallet.OutgoingTransactions.Select(x => x.ToEntity()).ToList(),
                        IncomingTransactions = playerWallet.IncomingTransactions.Select(x => x.ToEntity()).ToList()
                    };
                case JokerWallet jokerWallet:
                    return new WalletEntity {
                        Id = jokerWallet.Id,
                        GameId = jokerWallet.GameId,
                        Address = jokerWallet.Address,
                        StartAmount = jokerWallet.StartAmount,
                        PlayerId = null,
                        Type = WalletEntity.WalletType.JokerWallet.ToString(),
                        OutgoingTransactions = new List<OutgoingTransactionEntity>(),
                        IncomingTransactions = jokerWallet.IncomingTransactions.Select(x => x.ToEntity()).ToList()
                    };
                case CombinedTransactionWallet combinedTransactionWallet:
                    return new WalletEntity {
                        Id = combinedTransactionWallet.Id,
                        GameId = combinedTransactionWallet.GameId,
                        Address = combinedTransactionWallet.Address,
                        StartAmount = combinedTransactionWallet.StartAmount,
                        PlayerId = null,
                        Type = WalletEntity.WalletType.CombinedTransactionWallet.ToString(),
                        OutgoingTransactions = new List<OutgoingTransactionEntity>(),
                        IncomingTransactions = combinedTransactionWallet.IncomingTransactions.Select(x => x.ToEntity()).ToList()
                    };
                case LargeTransactionWallet largeTransactionWallet:
                    return new WalletEntity {
                        Id = largeTransactionWallet.Id,
                        GameId = largeTransactionWallet.GameId,
                        Address = largeTransactionWallet.Address,
                        StartAmount = largeTransactionWallet.StartAmount,
                        PlayerId = null,
                        Type = WalletEntity.WalletType.LargeTransactionWallet.ToString(),
                        OutgoingTransactions = new List<OutgoingTransactionEntity>(),
                        IncomingTransactions = largeTransactionWallet.IncomingTransactions.Select(x => x.ToEntity()).ToList()
                    };
            }

            throw new Exception("Unable to map wallet to entity");
        }

        public static Wallet ToDomainModel(this WalletEntity walletEntity)
        {
            var walletType = MapWalletTypeFromString(walletEntity.Type);

            switch (walletType)
            {
                case WalletEntity.WalletType.JokerWallet:
                    return new JokerWallet {
                        Id = walletEntity.Id,
                        GameId = walletEntity.GameId,
                        StartAmount = walletEntity.StartAmount,
                        IncomingTransactions = walletEntity.IncomingTransactions?.Select(x => x.ToDomainModel()).ToList() ?? new List<IncomingTransaction>()
                    };
                case WalletEntity.WalletType.CombinedTransactionWallet:
                    return new CombinedTransactionWallet {
                        Id = walletEntity.Id,
                        GameId = walletEntity.GameId,
                        StartAmount = walletEntity.StartAmount,
                        IncomingTransactions = walletEntity.IncomingTransactions?.Select(x => x.ToDomainModel()).ToList() ?? new List<IncomingTransaction>()
                    };
                case WalletEntity.WalletType.LargeTransactionWallet:
                    return new LargeTransactionWallet {
                        Id = walletEntity.Id,
                        GameId = walletEntity.GameId,
                        StartAmount = walletEntity.StartAmount,
                        IncomingTransactions = walletEntity.IncomingTransactions?.Select(x => x.ToDomainModel()).ToList() ?? new List<IncomingTransaction>()
                    };
                case WalletEntity.WalletType.PlayerWallet:
                    if (!walletEntity.PlayerId.HasValue)
                    {
                        throw new Exception("Inconsistent player wallet found in database");
                    }
                    
                    return new PlayerWallet {
                        Id = walletEntity.Id,
                        GameId = walletEntity.GameId,
                        Address = walletEntity.Address,
                        StartAmount = walletEntity.StartAmount,
                        PlayerId = walletEntity.PlayerId.Value,
                        IncomingTransactions = walletEntity.IncomingTransactions?.Select(x => x.ToDomainModel()).ToList() ?? new List<IncomingTransaction>(),
                        OutgoingTransactions = walletEntity.OutgoingTransactions?.Select(x => x.ToDomainModel()).ToList() ?? new List<OutgoingTransaction>()
                    };
                default:
                    throw new Exception("Unexpected wallet type in database");
            }
        }

        public static WalletEntity.WalletType MapWalletTypeFromString(string typeString)
        {
            return (WalletEntity.WalletType) Enum.Parse(typeof(WalletEntity.WalletType), typeString);
        }
    }
}