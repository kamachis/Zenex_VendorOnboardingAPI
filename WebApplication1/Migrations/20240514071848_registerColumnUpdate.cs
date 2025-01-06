using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class registerColumnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateStatus",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "TypeofIndustry",
                table: "bp_vob");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateStatus",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeofIndustry",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
