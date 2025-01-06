using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class VendorOnboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bp_contact",
                table: "bp_contact");

            migrationBuilder.DropColumn(
                name: "AttachmentContents",
                table: "bp_id");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "bp_id");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "bp_id");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "bp_id");

            migrationBuilder.DropColumn(
                name: "Item",
                table: "bp_contact");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "bp_id",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bp_contact",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bp_contact",
                table: "bp_contact",
                columns: new[] { "TransID", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bp_contact",
                table: "bp_contact");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "bp_id",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentContents",
                table: "bp_id",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "bp_id",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "bp_id",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                table: "bp_id",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "bp_contact",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "bp_contact",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bp_contact",
                table: "bp_contact",
                columns: new[] { "TransID", "Item" });
        }
    }
}
