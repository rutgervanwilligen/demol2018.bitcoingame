using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeMol2018.BitcoinGame.Application.Services;
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

            if (player != null)
            {
                var wallet = _walletService.GetWalletByPlayerId(player.Id);
                
                return Clients.Caller.SendAsync("LoginResult", new {
                    loginSuccessful = true,
                    isAdmin = player.IsAdmin,
                    playerGuid = player.Id,
                    usersWalletAddress = wallet.Address,
                    usersCurrentBalance = wallet.GetBalance()
                });
            }

            return Clients.Caller.SendAsync("LoginResult", new {
                loginSuccessful = false
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
        
        public Task MakeTransaction(int senderWalletId, int receiverWalletId, int amount)
        {
            _transactionService.MakeTransaction(senderWalletId, receiverWalletId, amount);
            
            var connectionIdToIgnore = new List<String>();
            return Clients.All.SendAsync("IncrementCounter");
//            connectionIdToIgnore.Add(Context.ConnectionId);
//            return Clients.AllExcept(connectionIdToIgnore).SendAsync("IncrementCounter");
        }
    }
}
