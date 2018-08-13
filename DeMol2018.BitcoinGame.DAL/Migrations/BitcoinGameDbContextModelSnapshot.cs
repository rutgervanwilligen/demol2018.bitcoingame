﻿// <auto-generated />
using System;
using DeMol2018.BitcoinGame.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    [DbContext(typeof(BitcoinGameDbContext))]
    partial class BitcoinGameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasFinished");

                    b.Property<bool>("IsCurrentGame");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.IncomingTransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<Guid>("ReceiverWalletId");

                    b.Property<int>("RoundNumber");

                    b.Property<Guid>("SenderWalletId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverWalletId");

                    b.ToTable("IncomingTransactions");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.OutgoingTransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int?>("InvalidReceiverAddress");

                    b.Property<Guid?>("ReceiverWalletId");

                    b.Property<int>("RoundNumber");

                    b.Property<Guid>("SenderWalletId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SenderWalletId");

                    b.ToTable("OutgoingTransactions");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.PlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAdmin");

                    b.Property<int>("LoginCode");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("WalletAddress");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.RoundEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GameId");

                    b.Property<int>("RoundNumber");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Address");

                    b.Property<Guid>("GameId");

                    b.Property<Guid?>("PlayerId");

                    b.Property<int>("StartAmount");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.IncomingTransactionEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", "ReceiverWallet")
                        .WithMany("IncomingTransactions")
                        .HasForeignKey("ReceiverWalletId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.OutgoingTransactionEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", "SenderWallet")
                        .WithMany("OutgoingTransactions")
                        .HasForeignKey("SenderWalletId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.RoundEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.GameEntity", "Game")
                        .WithMany("Rounds")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.GameEntity", "Game")
                        .WithMany("Wallets")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.PlayerEntity", "Player")
                        .WithMany("Wallets")
                        .HasForeignKey("PlayerId");
                });
#pragma warning restore 612, 618
        }
    }
}
