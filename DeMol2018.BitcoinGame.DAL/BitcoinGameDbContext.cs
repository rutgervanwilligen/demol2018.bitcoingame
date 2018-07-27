﻿using DeMol2018.BitcoinGame.DAL.Entities;
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
                .Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

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
                .Entity<OutgoingTransactionEntity>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<OutgoingTransactionEntity>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<OutgoingTransactionEntity>()
                .HasOne(x => x.SenderWallet)
                .WithMany(x => x.OutgoingTransactions)
                .HasForeignKey(x => x.SenderWalletId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder
                .Entity<IncomingTransactionEntity>()
                .HasKey(x => x.Id);

            modelBuilder
                .Entity<IncomingTransactionEntity>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<IncomingTransactionEntity>()
                .HasOne(x => x.ReceiverWallet)
                .WithMany(x => x.IncomingTransactions)
                .HasForeignKey(x => x.ReceiverWalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
