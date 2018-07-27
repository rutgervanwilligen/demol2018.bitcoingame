﻿using System;
using System.Threading.Tasks;
using DeMol2018.BitcoinGame.Application.Services;
using DeMol2018.BitcoinGame.ReactApp.ResultObjects;
using Microsoft.AspNetCore.SignalR;

namespace DeMol2018.BitcoinGame.ReactApp.Controllers
{
    public class BitcoinGameHub : Hub
    {
        private readonly TransactionService _transactionService;
        private readonly PlayerService _playerService;
        private readonly WalletService _walletService;
        private readonly GameService _gameService;
        private readonly RoundService _roundService;

        public BitcoinGameHub(
                TransactionService transactionService,
                PlayerService playerService,
                WalletService walletService,
                GameService gameService,
                RoundService roundService)
        {
            _transactionService = transactionService;
            _playerService = playerService;
            _walletService = walletService;
            _roundService = roundService;
            _gameService = gameService;
        }

        public Task Login(string name, int code)
        {
            var player = _playerService.Login(name, code);

            if (player == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = false
                });
            }

            var wallet = _walletService.GetWalletByPlayerId(player.Id);
            var currentRound = _roundService.GetCurrentRound();

            return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                LoginSuccessful = true,
                IsAdmin = player.IsAdmin,
                PlayerGuid = player.Id,
                UpdatedState = new UpdatedStateResult {
                    UserWalletAddress = wallet.Address,
                    UserCurrentBalance = currentRound == null ? wallet.StartAmount : wallet.GetCurrentBalanceInRound(currentRound.RoundNumber),
                    CurrentRoundEndTime = currentRound?.EndTime,
                    CurrentRoundNumber = currentRound?.RoundNumber
                }
            });
        }

        public Task StartNewRound(Guid invokerId, int lengthOfNewRoundInMinutes)
        {
            var player = _playerService.GetById(invokerId);

            if (!player.IsAdmin)
            {
                return Clients.Caller.SendAsync("StartNewRoundResult", new {
                    callSuccessful = false
                });
            }

            var newRound = _roundService.StartNewRound(TimeSpan.FromMinutes(lengthOfNewRoundInMinutes));

            return Clients.All.SendAsync("StartNewRoundResult", new {
                callSuccessful = true,
                newRoundNumber = newRound.RoundNumber,
                newRoundEndTime = newRound.EndTime
            });
        }
        
        public Task MakeTransaction(Guid invokerId, int receiverWalletAddress, int amount)
        {
            var currentRound = _roundService.GetCurrentRound();

            if (currentRound == null)
            {
                return Clients.Caller.SendAsync("MakeTransactionResult", new {
                    transactionSuccessful = false
                });
            }

            var senderWallet = _walletService.GetWalletByPlayerId(invokerId);

            var transaction = _transactionService.MakeTransaction(senderWallet.Address, receiverWalletAddress, amount);

            var updatedSenderWallet = _walletService.GetWalletByPlayerId(invokerId);

            return Clients.Caller.SendAsync("MakeTransactionResult", new {
                transactionSuccessful = transaction != null,
                userCurrentBalance = updatedSenderWallet.GetCurrentBalanceInRound(currentRound.RoundNumber)
            });
        }
    }
}
