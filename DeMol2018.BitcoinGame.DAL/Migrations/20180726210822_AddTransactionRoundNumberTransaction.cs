using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class AddTransactionRoundNumberTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundNumber",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundNumber",
                table: "Transactions");
        }
    }
}
