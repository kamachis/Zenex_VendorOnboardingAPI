using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class AddedBankDetailsRequiredFieldInbp_vob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BankDetailsRequired",
                table: "bp_vob",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankDetailsRequired",
                table: "bp_vob");
        }
    }
}
