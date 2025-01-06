using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class CoulmnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmamiContactPerson",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "EmamiContactPersonMail",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "MSME",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "RP",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "RP_Att_ID",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "RP_Name",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "RP_Type",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "Reduced_TDS",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "TDS_Att_ID",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "TDS_RATE",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "bp_bank");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "bp_bank");

            migrationBuilder.DropColumn(
                name: "DocID",
                table: "bp_bank");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "bp_bank");

            migrationBuilder.RenameColumn(
                name: "UploadDateTime",
                table: "bp_id",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "bp_id",
                newName: "UploadDateTime");

            migrationBuilder.AddColumn<string>(
                name: "EmamiContactPerson",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmamiContactPersonMail",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MSME",
                table: "bp_vob",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RP",
                table: "bp_vob",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RP_Att_ID",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RP_Name",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RP_Type",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reduced_TDS",
                table: "bp_vob",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TDS_Att_ID",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TDS_RATE",
                table: "bp_vob",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "bp_bank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "bp_bank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocID",
                table: "bp_bank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "bp_bank",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
