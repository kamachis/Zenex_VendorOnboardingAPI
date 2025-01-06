using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenex.Migrations
{
    public partial class UpdateAddressFieldColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "bp_vob",
                newName: "AddressLine5");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "bp_vob",
                newName: "AddressLine4");

            migrationBuilder.RenameColumn(
                name: "LandMark",
                table: "bp_vob",
                newName: "AddressLine3");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "bp_vob",
                newName: "AddressLine2");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "bp_vob",
                newName: "AddressLine1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine5",
                table: "bp_vob",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "AddressLine4",
                table: "bp_vob",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "AddressLine3",
                table: "bp_vob",
                newName: "LandMark");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "bp_vob",
                newName: "BuildingNumber");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "bp_vob",
                newName: "Area");
        }
    }
}
