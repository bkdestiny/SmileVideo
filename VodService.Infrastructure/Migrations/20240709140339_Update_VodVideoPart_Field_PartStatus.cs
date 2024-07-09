using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VodService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_VodVideoPart_Field_PartStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "VodVideoPart");

            migrationBuilder.AddColumn<int>(
                name: "PartStatus",
                table: "VodVideoPart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartStatus",
                table: "VodVideoPart");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "VodVideoPart",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
