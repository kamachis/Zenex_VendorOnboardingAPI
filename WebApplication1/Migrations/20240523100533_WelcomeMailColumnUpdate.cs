using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class WelcomeMailColumnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "VendorDetails",
                newName: "ToMailId");

            migrationBuilder.AddColumn<string>(
                name: "CCMailId",
                table: "VendorDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCMailId",
                table: "VendorDetails");

            migrationBuilder.RenameColumn(
                name: "ToMailId",
                table: "VendorDetails",
                newName: "Mail");
        }
    }
}
