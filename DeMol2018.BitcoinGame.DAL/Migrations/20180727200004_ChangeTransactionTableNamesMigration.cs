using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class ChangeTransactionTableNamesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingTransactionEntity_Wallets_ReceiverWalletId",
                table: "IncomingTransactionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingTransactionEntity_Wallets_SenderWalletId",
                table: "OutgoingTransactionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingTransactionEntity",
                table: "OutgoingTransactionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingTransactionEntity",
                table: "IncomingTransactionEntity");

            migrationBuilder.RenameTable(
                name: "OutgoingTransactionEntity",
                newName: "OutgoingTransactions");

            migrationBuilder.RenameTable(
                name: "IncomingTransactionEntity",
                newName: "IncomingTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingTransactionEntity_SenderWalletId",
                table: "OutgoingTransactions",
                newName: "IX_OutgoingTransactions_SenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomingTransactionEntity_ReceiverWalletId",
                table: "IncomingTransactions",
                newName: "IX_IncomingTransactions_ReceiverWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingTransactions",
                table: "OutgoingTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingTransactions",
                table: "IncomingTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingTransactions_Wallets_ReceiverWalletId",
                table: "IncomingTransactions",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingTransactions_Wallets_SenderWalletId",
                table: "OutgoingTransactions",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingTransactions_Wallets_ReceiverWalletId",
                table: "IncomingTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingTransactions_Wallets_SenderWalletId",
                table: "OutgoingTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutgoingTransactions",
                table: "OutgoingTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingTransactions",
                table: "IncomingTransactions");

            migrationBuilder.RenameTable(
                name: "OutgoingTransactions",
                newName: "OutgoingTransactionEntity");

            migrationBuilder.RenameTable(
                name: "IncomingTransactions",
                newName: "IncomingTransactionEntity");

            migrationBuilder.RenameIndex(
                name: "IX_OutgoingTransactions_SenderWalletId",
                table: "OutgoingTransactionEntity",
                newName: "IX_OutgoingTransactionEntity_SenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomingTransactions_ReceiverWalletId",
                table: "IncomingTransactionEntity",
                newName: "IX_IncomingTransactionEntity_ReceiverWalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutgoingTransactionEntity",
                table: "OutgoingTransactionEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingTransactionEntity",
                table: "IncomingTransactionEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingTransactionEntity_Wallets_ReceiverWalletId",
                table: "IncomingTransactionEntity",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingTransactionEntity_Wallets_SenderWalletId",
                table: "OutgoingTransactionEntity",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
