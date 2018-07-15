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

        public TransactionService(BitcoinGameDbContext dbContext)
        {
            TransactionRepository = new TransactionRepository(dbContext);
            WalletRepository = new WalletRepository(dbContext);
        }

        public void MakeTransaction(int senderWalletAddress, int receiverWalletAddress, int amount)
        {
            var senderWallet = WalletRepository.GetBy(x => x.Address == senderWalletAddress).ToDomainModel();

            var receiverWalletEntity = WalletRepository.FindBy(x => x.Address == receiverWalletAddress);

            if (receiverWalletEntity != null)
            {
                var receiverWallet = receiverWalletEntity.ToDomainModel();
                
                var transaction = new Transaction {
                    Amount = amount,
                    Sender = senderWallet,
                    Receiver = receiverWallet
                };

                TransactionRepository.Add(transaction.ToEntity());
                TransactionRepository.SaveChanges();
            }
            
            
        }
        
        
    }
}