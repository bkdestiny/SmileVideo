using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VodService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VodVideoComment_RemoveField_VideoCommentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoCommentTypes",
                table: "VodVideoComment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoCommentTypes",
                table: "VodVideoComment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
