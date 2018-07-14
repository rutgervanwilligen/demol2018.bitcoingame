using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class AddHasFinishedToGameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.AddColumn<bool>(
                name: "HasFinished",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFinished",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "Value");
        }
    }
}
