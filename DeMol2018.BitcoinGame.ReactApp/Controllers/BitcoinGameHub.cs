using System;
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
            var player = _playerService.Login(name, code);

            if (player == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = false
                });
            }

            var currentGame = _gameService.FindCurrentGame();
            var currentRound = _gameService.GetCurrentRound();

            if (currentGame == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = true,
                    IsAdmin = player.IsAdmin,
                    PlayerGuid = player.Id,
                    UpdatedState = new UpdatedStateResult()
                });
            }

            var userWalletAddress = 0;
            var userCurrentBalance = 0;

            var nonPlayerWallets = _walletService.GetNonPlayerWalletsByGameId(currentGame.Id);
            var nonPlayerWalletsResult = nonPlayerWallets
                .Select(x => NonPlayerWalletResult.GetResultFromWalletAndRound(x, currentRound?.RoundNumber))
                .ToArray();

            var moneyWonSoFar =
                _walletService.GetMoneyWonSoFarInGameIdAndRound(currentGame.Id, currentRound?.RoundNumber);

            if (!player.IsAdmin)
            {
                var wallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, player.Id);
                userWalletAddress = wallet.Address;
                userCurrentBalance = currentRound == null
                    ? wallet.StartAmount
                    : wallet.GetCurrentBalanceInRound(currentRound.RoundNumber);
            }

            return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                LoginSuccessful = true,
                IsAdmin = player.IsAdmin,
                PlayerGuid = player.Id,
                UpdatedState = new UpdatedStateResult {
                    CurrentGameId = currentGame?.Id,
                    UserWalletAddress = userWalletAddress,
                    UserCurrentBalance = userCurrentBalance,
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber,
                    NonPlayerWallets = nonPlayerWalletsResult,
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
            var currentRound = _gameService.GetCurrentRound();

            if (currentGame == null)
            {
                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = true,
                    UpdatedState = new UpdatedStateResult()
                });
            }

            var nonPlayerWallets = _walletService.GetNonPlayerWalletsByGameId(currentGame.Id);
            var nonPlayerWalletsResult = nonPlayerWallets
                .Select(x => NonPlayerWalletResult.GetResultFromWalletAndRound(x, currentRound?.RoundNumber))
                .ToArray();

            var moneyWonSoFar =
                _walletService.GetMoneyWonSoFarInGameIdAndRound(currentGame.Id, currentRound?.RoundNumber);

            if (player.IsAdmin)
            {
                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = true,
                    UpdatedState = new UpdatedStateResult {
                        CurrentGameId = currentGame.Id,
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
                    UserWalletAddress = wallet.Address,
                    UserCurrentBalance = currentRound == null 
                        ? wallet.StartAmount
                        : wallet.GetCurrentBalanceInRound(currentRound.RoundNumber),
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

            _gameService.StartNewGame();

            var currentGame = _gameService.FindCurrentGame();
            _playerService.createNewWalletsForGame(currentGame.Id);

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
                    userCurrentBalance = senderWallet.GetCurrentBalanceInRound(currentRound.RoundNumber)
                });
            }

            var updatedSenderWallet = _walletService.GetWalletByGameIdAndPlayerId(currentRound.GameId, invokerId);

            return Clients.Caller.SendAsync("MakeTransactionResult", new {
                transactionSuccessful = true,
                userCurrentBalance = updatedSenderWallet.GetCurrentBalanceInRound(currentRound.RoundNumber)
            });
        }
    }
}
