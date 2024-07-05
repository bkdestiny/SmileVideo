using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VodService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVodVideoFieldIsPublicToVideoStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "VodVideo");

            migrationBuilder.AddColumn<int>(
                name: "VideoStatus",
                table: "VodVideo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoStatus",
                table: "VodVideo");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "VodVideo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
