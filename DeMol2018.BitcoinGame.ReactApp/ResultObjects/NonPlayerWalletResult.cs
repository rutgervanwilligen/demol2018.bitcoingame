﻿using System;
using DeMol2018.BitcoinGame.Domain.Models.Wallets;

namespace DeMol2018.BitcoinGame.ReactApp.ResultObjects
{
    public class NonPlayerWalletResult
    {
        public int Address { get; set; }
        public int CurrentBalance { get; set; }
        public string Name { get; set; }

        public static NonPlayerWalletResult GetResultFromWalletAndRound(Wallet wallet, int? roundNumber)
        {
            switch (wallet)
            {
                case JokerWallet jokerWallet:
                    return new NonPlayerWalletResult {
                        Address = jokerWallet.Address,
                        CurrentBalance = roundNumber == null
                            ? jokerWallet.StartAmount
                            : jokerWallet.GetCurrentBalanceInRound(roundNumber.Value),
                        Name = jokerWallet.DisplayName
                    };
                case LargeTransactionWallet largeTransactionWallet:
                    return new NonPlayerWalletResult {
                        Address = largeTransactionWallet.Address,
                        CurrentBalance = roundNumber == null
                            ? largeTransactionWallet.StartAmount
                            : largeTransactionWallet.GetCurrentBalanceInRound(roundNumber.Value),
                        Name = largeTransactionWallet.DisplayName
                    };
                case CombinedTransactionWallet combinedTransactionWallet:
                    return new NonPlayerWalletResult {
                        Address = combinedTransactionWallet.Address,
                        CurrentBalance = roundNumber == null
                            ? combinedTransactionWallet.StartAmount
                            : combinedTransactionWallet.GetCurrentBalanceInRound(roundNumber.Value),
                        Name = combinedTransactionWallet.DisplayName
                    };
            }

            throw new Exception("Invalid wallet type");
        }
    }
}