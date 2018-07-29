using System;
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
        private readonly RoundService _roundService;

        public BitcoinGameHub(
                TransactionService transactionService,
                PlayerService playerService,
                WalletService walletService,
                RoundService roundService)
        {
            _transactionService = transactionService;
            _playerService = playerService;
            _walletService = walletService;
            _roundService = roundService;
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
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber
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

            var wallet = _walletService.GetWalletByPlayerId(player.Id);
            var currentRound = _roundService.GetCurrentRound();

            return Clients.Caller.SendAsync("FetchNewGameStateResult", new FetchNewGameStateResult {
                CallSuccessful = true,
                UpdatedState = new UpdatedStateResult {
                    UserWalletAddress = wallet.Address,
                    UserCurrentBalance = currentRound == null ? wallet.StartAmount : wallet.GetCurrentBalanceInRound(currentRound.RoundNumber),
                    CurrentRoundEndTime = currentRound?.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    CurrentRoundNumber = currentRound?.RoundNumber
                }
            });
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

            _roundService.StartNewRound(TimeSpan.FromMinutes(lengthOfNewRoundInMinutes));

            return Clients.All.SendAsync("AnnounceNewRoundResult");
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

            var updatedSenderWallet = _walletService.GetWalletByPlayerId(invokerId);

            return Clients.Caller.SendAsync("MakeTransactionResult", new {
                transactionSuccessful = true,
                userCurrentBalance = updatedSenderWallet.GetCurrentBalanceInRound(currentRound.RoundNumber)
            });
        }
    }
}
