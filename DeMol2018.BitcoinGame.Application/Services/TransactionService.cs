using System;
using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class TransactionService
    {
        private WalletRepository WalletRepository { get; }
        private RoundService RoundService { get; }
        private GameService GameService { get; }

        public TransactionService(RoundService roundService, GameService gameService, BitcoinGameDbContext dbContext)
        {
            WalletRepository = new WalletRepository(dbContext);
            RoundService = roundService;
            GameService = gameService;
        }

        public void MakeTransaction(int senderWalletAddress, int receiverWalletAddress, int amount)
        {
            var currentRound = RoundService.GetCurrentRound();
            var currentGame = GameService.FindCurrentGame();

            if (currentRound == null)
            {
                throw new InvalidTransactionException("There is no active round. Transaction failed.");
            }
            
            var senderWalletEntity = WalletRepository.GetBy(x => x.Address == senderWalletAddress);
            var senderWallet = senderWalletEntity.ToDomainModel();
            var receiverWalletEntity = WalletRepository.FindBy(x => x.Address == receiverWalletAddress);
            var receiverWallet = receiverWalletEntity?.ToDomainModel();

            var outgoingTransaction = senderWallet.MakeTransaction(
                receiverWallet?.Id,
                amount,
                currentGame.Id,
                currentRound.Id,
                currentRound.RoundNumber,
                receiverWallet == null ? receiverWalletAddress : (int?)null);
            
            senderWalletEntity.OutgoingTransactions.Add(outgoingTransaction.ToEntity());
            
            WalletRepository.Update(senderWalletEntity);
            WalletRepository.SaveChanges();

            if (receiverWallet == null) {
                return;
            }

            var incomingTransaction = receiverWallet.AddIncomingTransaction(
                currentGame.Id,
                currentRound.Id,
                currentRound.RoundNumber,
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