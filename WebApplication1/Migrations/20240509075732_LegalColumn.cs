using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class LegalColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "PrimaryContactPerson",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "SecondaryContactPerson",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "bp_bank");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactPerson",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactPerson",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "bp_bank",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
