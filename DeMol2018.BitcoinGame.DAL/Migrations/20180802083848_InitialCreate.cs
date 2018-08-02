using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasFinished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LoginCode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    StartAmount = table.Column<int>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: true),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReceiverWalletId = table.Column<Guid>(nullable: false),
                    SenderWalletId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingTransactions_Wallets_ReceiverWalletId",
                        column: x => x.ReceiverWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderWalletId = table.Column<Guid>(nullable: false),
                    ReceiverWalletId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false),
                    InvalidReceiverAddress = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingTransactions_Wallets_SenderWalletId",
                        column: x => x.SenderWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingTransactions_ReceiverWalletId",
                table: "IncomingTransactions",
                column: "ReceiverWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingTransactions_SenderWalletId",
                table: "OutgoingTransactions",
                column: "SenderWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_GameId",
                table: "Rounds",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_GameId",
                table: "Wallets",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_PlayerId",
                table: "Wallets",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingTransactions");

            migrationBuilder.DropTable(
                name: "OutgoingTransactions");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
