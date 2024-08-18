using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LiveRoom_AddColumn_CoverFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CoverFile",
                table: "LiveRoom",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverFile",
                table: "LiveRoom");
        }
    }
}
