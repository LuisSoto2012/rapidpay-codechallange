using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidPay.Data.Migrations
{
    public partial class AlterTable_Card_AddIdentificationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentificationNumber",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificationNumber",
                table: "Cards");
        }
    }
}
