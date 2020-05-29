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
            var player = _playerService.Login(name, code);

            if (player == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = false
                });
            }

            var currentGame = _gameService.FindCurrentGame();

            if (currentGame == null)
            {
                return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                    LoginSuccessful = true,
                    IsAdmin = player.IsAdmin,
                    PlayerGuid = player.Id,
                    UpdatedState = new UpdatedStateResult()
                });
            }

            var currentRound = currentGame.GetCurrentRound();
            var lastRoundNumber = currentGame.GetLastFinishedRoundNumber();

            var userWalletAddress = 0;
            var userCurrentBalance = 0;
            int? numberOfJokersWon = null;

            var nonPlayerWallets = _walletService.GetNonPlayerWalletsByGameId(currentGame.Id);
            var nonPlayerWalletsState = nonPlayerWallets
                .Select(x => NonPlayerWalletResult.GetNonPlayerWalletStatesAfterRound(x, lastRoundNumber))
                .ToArray();

            var moneyWonSoFar =
                _walletService.GetMoneyWonSoFarInGameIdUpUntilRound(currentGame.Id, lastRoundNumber);

            if (!player.IsAdmin)
            {
                var wallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, player.Id);
                userWalletAddress = wallet.Address;
                userCurrentBalance = lastRoundNumber == null
                    ? wallet.GetBalanceAfterRound(0)
                    : wallet.GetBalanceAfterRound(lastRoundNumber.Value);
            }

            var jokerWinnersResult = new List<JokerWinnerResult>();

            if (currentGame.HasFinished)
            {
                if (player.IsAdmin)
                {
                    var jokerWinnerWallets = _walletService.GetJokerWinnersInGame(currentGame.Id);
                    jokerWinnersResult = jokerWinnerWallets.Select(JokerWinnerResult.MapFrom).ToList();
                }
                else
                {
                    var jokerWinners = _walletService.GetJokerWinnersInGame(currentGame.Id);
                    numberOfJokersWon = jokerWinners.Single(x => x.PlayerId == player.Id).NumberOfJokersWon;
                }
            }

            return Clients.Caller.SendAsync("LoginResult", new LoginResult {
                LoginSuccessful = true,
                IsAdmin = player.IsAdmin,
                PlayerGuid = player.Id,
                UpdatedState = new UpdatedStateResult {
                    CurrentGameId = currentGame.Id,
                    GameHasFinished = currentGame.HasFinished,
                    LastRoundNumber = lastRoundNumber,
                    UserWalletAddress = userWalletAddress,
                    UserCurrentBalance = userCurrentBalance,
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber,
                    NonPlayerWallets = nonPlayerWalletsState,
                    MoneyWonSoFar = moneyWonSoFar,
                    NumberOfJokersWon = numberOfJokersWon,
                    JokerWinners = jokerWinnersResult.ToArray()
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
                var jokerWinners = new List<JokerWinnerResult>();

                if (currentGame.HasFinished)
                {
                    var jokerWinnerWallets = _walletService.GetJokerWinnersInGame(currentGame.Id);
                    jokerWinners = jokerWinnerWallets.Select(JokerWinnerResult.MapFrom).ToList();
                }

                return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                    CallSuccessful = true,
                    UpdatedState = new UpdatedStateResult {
                        CurrentGameId = currentGame.Id,
                        GameHasFinished = currentGame.HasFinished,
                        LastRoundNumber = lastRoundNumber,
                        CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        CurrentRoundNumber = currentRound?.RoundNumber,
                        NonPlayerWallets = nonPlayerWalletsResult,
                        MoneyWonSoFar = moneyWonSoFar,
                        JokerWinners = jokerWinners.ToArray()
                    }
                });
            }

            var wallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, player.Id);
            int? numberOfJokersWon = null;

            var jokerWinnersResult = new List<JokerWinnerResult>();

            if (currentGame.HasFinished)
            {
                var jokerWinners = _walletService.GetJokerWinnersInGame(currentGame.Id);
                numberOfJokersWon = jokerWinners.Single(x => x.PlayerId == player.Id).NumberOfJokersWon;

                if (player.IsAdmin)
                {
                    var jokerWinnerWallets = _walletService.GetJokerWinnersInGame(currentGame.Id);
                    jokerWinnersResult = jokerWinnerWallets.Select(JokerWinnerResult.MapFrom).ToList();
                }
            }

            return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                CallSuccessful = true,
                UpdatedState = new UpdatedStateResult {
                    CurrentGameId = currentGame.Id,
                    GameHasFinished = currentGame.HasFinished,
                    LastRoundNumber = lastRoundNumber,
                    UserWalletAddress = wallet.Address,
                    UserCurrentBalance = lastRoundNumber == null
                        ? wallet.GetBalanceAfterRound(0)
                        : wallet.GetBalanceAfterRound(lastRoundNumber.Value),
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber,
                    NonPlayerWallets = nonPlayerWalletsResult,
                    MoneyWonSoFar = moneyWonSoFar,
                    NumberOfJokersWon = numberOfJokersWon,
                    JokerWinners = jokerWinnersResult.ToArray()
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

            _playerService.CreateNewWalletsForGame(newGame.Id);

            return Clients.All.SendAsync("AnnounceNewGameStateResult");
        }

        public Task FinishCurrentGame(Guid invokerId)
        {
            var player = _playerService.GetById(invokerId);

            if (!player.IsAdmin)
            {
                return Clients.Caller.SendAsync("FinishCurrentGameResult", new FinishCurrentGameResult {
                    CallSuccessful = false
                });
            }

            _gameService.FinishCurrentGame();

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
            var currentGame = _gameService.FindCurrentGame();

            if (currentGame?.GetCurrentRound() == null)
            {
                return Clients.Caller.SendAsync("MakeTransactionResult", new {
                    transactionSuccessful = false
                });
            }

            var currentRoundNumber = currentGame.GetCurrentRound().RoundNumber;
            var senderWallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, invokerId);

            try
            {
                _transactionService.MakeTransaction(senderWallet.Address, receiverWalletAddress, amount);
            }
            catch (InvalidTransactionException)
            {
                return Clients.Caller.SendAsync("MakeTransactionResult", new {
                    transactionSuccessful = false,
                    userCurrentBalance = senderWallet.GetBalanceAfterRound(currentRoundNumber - 1)
                });
            }

            var updatedSenderWallet = _walletService.GetWalletByGameIdAndPlayerId(currentGame.Id, invokerId);

            return Clients.Caller.SendAsync("MakeTransactionResult", new {
                transactionSuccessful = true,
                userCurrentBalance = updatedSenderWallet.GetBalanceAfterRound(currentRoundNumber - 1)
            });
        }
    }
}
