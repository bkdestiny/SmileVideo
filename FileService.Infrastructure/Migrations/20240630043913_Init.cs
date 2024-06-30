using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FileContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileSHA256Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RemoteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysFile_FileSHA256Hash_FileSize",
                table: "SysFile",
                columns: new[] { "FileSHA256Hash", "FileSize" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysFile");
        }
    }
}
