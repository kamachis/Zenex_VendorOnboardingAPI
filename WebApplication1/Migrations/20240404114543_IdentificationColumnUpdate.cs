using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class IdentificationColumnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "bp_id",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDateTime",
                table: "bp_id",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "bp_id");

            migrationBuilder.DropColumn(
                name: "UploadDateTime",
                table: "bp_id");
        }
    }
}
