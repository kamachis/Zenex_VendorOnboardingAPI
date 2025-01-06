using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class identityAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegexFormat",
                table: "cbp_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "cbp_id",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "MaxSizeInKB",
                table: "cbp_id",
                newName: "maxSizeInKB");

            migrationBuilder.RenameColumn(
                name: "Mandatory",
                table: "cbp_id",
                newName: "mandatory");

            migrationBuilder.RenameColumn(
                name: "FileFormat",
                table: "cbp_id",
                newName: "fileFormat");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "cbp_id",
                newName: "country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "cbp_id",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "maxSizeInKB",
                table: "cbp_id",
                newName: "MaxSizeInKB");

            migrationBuilder.RenameColumn(
                name: "mandatory",
                table: "cbp_id",
                newName: "Mandatory");

            migrationBuilder.RenameColumn(
                name: "fileFormat",
                table: "cbp_id",
                newName: "FileFormat");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "cbp_id",
                newName: "Text");

            migrationBuilder.AddColumn<string>(
                name: "RegexFormat",
                table: "cbp_id",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
