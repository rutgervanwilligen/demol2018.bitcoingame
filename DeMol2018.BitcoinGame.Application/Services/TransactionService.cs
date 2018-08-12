using System;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class TransactionService
    {
        private WalletRepository WalletRepository { get; }
        private GameService GameService { get; }

        public TransactionService(GameService gameService, BitcoinGameDbContext dbContext)
        {
            WalletRepository = new WalletRepository(dbContext);
            GameService = gameService;
        }

        public void MakeTransaction(int senderWalletAddress, int receiverWalletAddress, int amount)
        {
            var currentGame = GameService.FindCurrentGame();

            if (currentGame?.GetCurrentRound() == null)
            {
                throw new InvalidTransactionException("There is no active round. Transaction failed.");
            }

            var currentRoundNumber = currentGame.GetCurrentRound().RoundNumber;
            
            var senderWalletEntity = WalletRepository.GetBy(x => x.Address == senderWalletAddress && x.GameId == currentGame.Id);
            var senderWallet = senderWalletEntity.ToDomainModel();

            if (amount <= 0)
            {
                throw new InvalidTransactionException("Transaction amount should be > 0");
            }

            var receiverWalletEntity = WalletRepository.FindBy(x => x.Address == receiverWalletAddress && x.GameId == currentGame.Id);
            var receiverWallet = receiverWalletEntity?.ToDomainModel();

            var outgoingTransaction = senderWallet.MakeTransaction(
                receiverWallet?.Id,
                amount,
                currentRoundNumber,
                receiverWallet == null ? receiverWalletAddress : (int?)null);
            
            senderWalletEntity.OutgoingTransactions.Add(outgoingTransaction.ToEntity());
            
            WalletRepository.Update(senderWalletEntity);
            WalletRepository.SaveChanges();

            if (receiverWallet == null) {
                return;
            }

            var incomingTransaction = receiverWallet.AddIncomingTransaction(
                currentRoundNumber,
                amount,
                senderWallet.Id);
            
            receiverWalletEntity.IncomingTransactions.Add(incomingTransaction.ToEntity());
            
            WalletRepository.Update(receiverWalletEntity);
            WalletRepository.SaveChanges();
        }
    }

    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException(string message) : base(message)
        {
        }
    }
}