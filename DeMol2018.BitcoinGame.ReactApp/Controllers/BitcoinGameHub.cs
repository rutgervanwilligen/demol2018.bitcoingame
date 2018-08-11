using System;
using System.Collections.Generic;
using System.Linq;
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

        public BitcoinGameHub(
                TransactionService transactionService,
                PlayerService playerService,
                WalletService walletService,
                GameService gameService)
        {
            _transactionService = transactionService;
            _playerService = playerService;
            _walletService = walletService;
            _gameService = gameService;
        }

        public Task Login(string name, int code)
        {
            var a = DateTime.UtcNow;
            var player = _playerService.Login(name, code);

            var b = DateTime.UtcNow;
            if (player == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = false
                });
            }

            var c = DateTime.UtcNow;
            var currentGame = _gameService.FindCurrentGame();

            var d = DateTime.UtcNow;
            if (currentGame == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = true,
                    IsAdmin = player.IsAdmin,
                    PlayerGuid = player.Id,
                    UpdatedState = new UpdatedStateResult()
                });
            }

            var e = DateTime.UtcNow;
            var currentRound = currentGame.GetCurrentRound();
            var lastRoundNumber = currentGame.GetLastFinishedRoundNumber();

            var f = DateTime.UtcNow;
            var userWalletAddress = 0;
            var userCurrentBalance = 0;

            var nonPlayerWallets = _walletService.GetNonPlayerWalletsByGameId(currentGame.Id);
            var nonPlayerWalletsState = nonPlayerWallets
                .Select(x => NonPlayerWalletResult.GetNonPlayerWalletStatesAfterRound(x, lastRoundNumber))
                .ToArray();

            var g = DateTime.UtcNow;
            var moneyWonSoFar =
                _walletService.GetMoneyWonSoFarInGameIdUpUntilRound(currentGame.Id, lastRoundNumber);

            var h = DateTime.UtcNow;
            if (!player.IsAdmin)
            {
                var wallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, player.Id);
                userWalletAddress = wallet.Address;
                userCurrentBalance = lastRoundNumber == null
                    ? wallet.StartAmount
                    : wallet.GetBalanceAfterRound(lastRoundNumber.Value);
            }

            var i = DateTime.UtcNow;
            return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                LoginSuccessful = true,
                IsAdmin = player.IsAdmin,
                PlayerGuid = player.Id,
                Tests = new List<DateTime> { a, b, c, d, e, f, g, h, i}.ToArray(),
                UpdatedState = new UpdatedStateResult {
                    CurrentGameId = currentGame.Id,
                    LastRoundNumber = lastRoundNumber,
                    UserWalletAddress = userWalletAddress,
                    UserCurrentBalance = userCurrentBalance,
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber,
                    NonPlayerWallets = nonPlayerWalletsState,
                    MoneyWonSoFar = moneyWonSoFar
                }
            });
        }

        public Task FetchNewGameState(Guid? invokerId)
        {
            if (!invokerId.HasValue)
            {
                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = false
                });
            }

            var player = _playerService.GetById(invokerId.Value);

            var currentGame = _gameService.FindCurrentGame();

            if (currentGame == null)
            {
                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = true,
                    UpdatedState = new UpdatedStateResult()
                });
            }

            var currentRound = currentGame.GetCurrentRound();
            var lastRoundNumber = currentGame.GetLastFinishedRoundNumber();

            var nonPlayerWallets = _walletService.GetNonPlayerWalletsByGameId(currentGame.Id);
            var nonPlayerWalletsResult = nonPlayerWallets
                .Select(x => NonPlayerWalletResult.GetNonPlayerWalletStatesAfterRound(x, lastRoundNumber))
                .ToArray();

            var moneyWonSoFar =
                _walletService.GetMoneyWonSoFarInGameIdUpUntilRound(currentGame.Id, lastRoundNumber);

            if (player.IsAdmin)
            {
                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = true,
                    UpdatedState = new UpdatedStateResult {
                        CurrentGameId = currentGame.Id,
                        LastRoundNumber = lastRoundNumber,
                        CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CurrentRoundNumber = currentRound?.RoundNumber,
                        NonPlayerWallets = nonPlayerWalletsResult,
                        MoneyWonSoFar = moneyWonSoFar
                    }
                });
            }

            var wallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, player.Id);

            return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                CallSuccessful = true,
                UpdatedState = new UpdatedStateResult {
                    CurrentGameId = currentGame.Id,
                    LastRoundNumber = lastRoundNumber,
                    UserWalletAddress = wallet.Address,
                    UserCurrentBalance = lastRoundNumber == null
                        ? wallet.StartAmount
                        : wallet.GetBalanceAfterRound(lastRoundNumber.Value),
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber,
                    NonPlayerWallets = nonPlayerWalletsResult,
                    MoneyWonSoFar = moneyWonSoFar
                }
            });
        }

        public Task StartNewGame(Guid invokerId)
        {
            var player = _playerService.GetById(invokerId);

            if (!player.IsAdmin)
            {
                return Clients.Caller.SendAsync("StartNewGameResult", new StartNewRoundResult {
                    CallSuccessful = false
                });
            }

            var newGame = _gameService.StartNewGame();

            _playerService.createNewWalletsForGame(newGame.Id);

            return Clients.All.SendAsync("AnnounceNewGameStateResult");
        }

        public Task StartNewRound(Guid invokerId, int lengthOfNewRoundInMinutes)
        {
            var player = _playerService.GetById(invokerId);

            if (!player.IsAdmin)
            {
                return Clients.Caller.SendAsync("StartNewRoundResult", new StartNewRoundResult {
                    CallSuccessful = false
                });
            }

            _gameService.StartNewRound(TimeSpan.FromMinutes(lengthOfNewRoundInMinutes));

            return Clients.All.SendAsync("AnnounceNewGameStateResult");
        }
        
        public Task MakeTransaction(Guid invokerId, int receiverWalletAddress, int amount)
        {
            var currentRound = _gameService.GetCurrentRound();

            if (currentRound == null)
            {
                return Clients.Caller.SendAsync("MakeTransactionResult", new {
                    transactionSuccessful = false
                });
            }

            var senderWallet = _walletService.GetWalletByGameIdAndPlayerId(currentRound.GameId, invokerId);

            try
            {
                _transactionService.MakeTransaction(senderWallet.Address, receiverWalletAddress, amount);
            }
            catch (InvalidTransactionException)
            {                
                return Clients.Caller.SendAsync("MakeTransactionResult", new {
                    transactionSuccessful = false,
                    userCurrentBalance = senderWallet.GetBalanceAfterRound(currentRound.RoundNumber - 1)
                });
            }

            var updatedSenderWallet = _walletService.GetWalletByGameIdAndPlayerId(currentRound.GameId, invokerId);

            return Clients.Caller.SendAsync("MakeTransactionResult", new {
                transactionSuccessful = true,
                userCurrentBalance = updatedSenderWallet.GetBalanceAfterRound(currentRound.RoundNumber - 1)
            });
        }
    }
}
