using DeMol2018.BitcoinGame.DAL;
using DeMol2018.BitcoinGame.DAL.Mappers;
using DeMol2018.BitcoinGame.DAL.Repositories;
using DeMol2018.BitcoinGame.Domain.Models;

namespace DeMol2018.BitcoinGame.Application.Services
{
    public class TransactionService
    {
        private WalletRepository WalletRepository { get; set; }
        private TransactionRepository TransactionRepository { get; set; }
        private RoundService RoundService { get; set; }

        public TransactionService(RoundService roundService, BitcoinGameDbContext dbContext)
        {
            TransactionRepository = new TransactionRepository(dbContext);
            WalletRepository = new WalletRepository(dbContext);
            RoundService = roundService;
        }

        public Transaction MakeTransaction(int senderWalletAddress, int receiverWalletAddress, int amount)
        {
            var currentRound = RoundService.GetCurrentRound();

            if (currentRound == null)
            {
                return null;
            }
            
            var senderWallet = WalletRepository.GetBy(x => x.Address == senderWalletAddress).ToDomainModel();
            var receiverWallet = WalletRepository.FindBy(x => x.Address == receiverWalletAddress)?.ToDomainModel();

            var transaction = new Transaction {
                Amount = amount,
                SenderId = senderWallet.Id,
                ReceiverId = receiverWallet?.Id,
                RoundId = currentRound.Id,
                InvalidReceiverAddress = receiverWallet == null ? receiverWalletAddress : (int?)null
            };

            TransactionRepository.Add(transaction.ToEntity());
            TransactionRepository.SaveChanges();

            return transaction;
        }
    }
}