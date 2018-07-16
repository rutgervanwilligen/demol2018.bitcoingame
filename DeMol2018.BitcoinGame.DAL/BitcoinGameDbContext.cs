using DeMol2018.BitcoinGame.DAL.Entities;
using DeMol2018.BitcoinGame.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DeMol2018.BitcoinGame.DAL
{
    public class BitcoinGameDbContext : DbContext
    {
        public BitcoinGameDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<RoundEntity> Rounds { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }
        public DbSet<GameEntity> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PlayerEntity>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<PlayerEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");
            
            modelBuilder
                .Entity<RoundEntity>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<RoundEntity>()
                .Property(x => x.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            modelBuilder
                .Entity<RoundEntity>()
                .Property(x => x.EndTime)
                .HasColumnType("datetime2")
                .IsRequired();

            modelBuilder
                .Entity<RoundEntity>()
                .HasOne(x => x.Game)
                .WithMany(x => x.Rounds)
                .HasForeignKey(x => x.GameId);

            modelBuilder
                .Entity<RoundEntity>()
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Round)
                .HasForeignKey(x => x.RoundId);
            
            modelBuilder
                .Entity<GameEntity>()
                .HasKey(x => x.Id);
            
            modelBuilder
                .Entity<GameEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");
            
            modelBuilder
                .Entity<GameEntity>()
                .Property(x => x.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            modelBuilder
                .Entity<WalletEntity>()
                .HasKey(x => x.Id);
            
            modelBuilder
                .Entity<WalletEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder
                .Entity<WalletEntity>()
                .HasOne(x => x.Player)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.PlayerId);

            modelBuilder
                .Entity<TransactionEntity>()
                .HasKey(x => x.Id);
            
            modelBuilder
                .Entity<TransactionEntity>()
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder
                .Entity<TransactionEntity>()
                .HasOne(x => x.Round)
                .WithMany(x => x.Transactions)
                .IsRequired()
                .HasForeignKey(x => x.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<TransactionEntity>()
                .HasOne(x => x.SenderWallet)
                .WithMany(x => x.SentTransactions)
                .IsRequired()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<TransactionEntity>()
                .HasOne(x => x.ReceiverWallet)
                .WithMany(x => x.ReceivedTransactions)
                .IsRequired()
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
