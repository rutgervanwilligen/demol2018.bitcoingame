using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeMol2018.BitcoinGame.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace DeMol2018.BitcoinGame.ReactApp.Controllers
{
    public class BitcoinGameHub : Hub
    {
        private readonly TransactionService _transactionService = new TransactionService();
        
        public Task MakeTransaction(int senderWalletId, int receiverWalletId, int amount)
        {
            _transactionService.MakeTransaction(senderWalletId, receiverWalletId, amount);
            
            List<String> connectionIdToIgnore = new List<String>();
            return Clients.All.SendAsync("IncrementCounter");
//            connectionIdToIgnore.Add(Context.ConnectionId);
//            return Clients.AllExcept(connectionIdToIgnore).SendAsync("IncrementCounter");
        }

        public Task DecrementCounter()
        {
            List<String> connectionIdToIgnore = new List<String>();
            connectionIdToIgnore.Add(Context.ConnectionId);
            return Clients.AllExcept(connectionIdToIgnore).SendAsync("DecrementCounter");
        }
    }
}
