using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VodService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VodVideo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CoverFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Performers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scriptwriter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodVideo", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "VodVideoClassify",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassifyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassifyType = table.Column<int>(type: "int", nullable: false),
                    SortIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodVideoClassify", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "VodVideoComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RespondentUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RootVideoCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoCommentTypes = table.Column<int>(type: "int", nullable: false),
                    LikesCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodVideoComment", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_VodVideoComment_VodVideoComment_RootVideoCommentId",
                        column: x => x.RootVideoCommentId,
                        principalTable: "VodVideoComment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VodVideoComment_VodVideo_VideoId",
                        column: x => x.VideoId,
                        principalTable: "VodVideo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VodVideoPart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartFile = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReleaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    SortIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodVideoPart", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_VodVideoPart_VodVideo_VideoId",
                        column: x => x.VideoId,
                        principalTable: "VodVideo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VodVideoClassifyRelation",
                columns: table => new
                {
                    VideoClassifiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodVideoClassifyRelation", x => new { x.VideoClassifiesId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_VodVideoClassifyRelation_VodVideoClassify_VideoClassifiesId",
                        column: x => x.VideoClassifiesId,
                        principalTable: "VodVideoClassify",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VodVideoClassifyRelation_VodVideo_VideosId",
                        column: x => x.VideosId,
                        principalTable: "VodVideo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VodVideoClassifyRelation_VideosId",
                table: "VodVideoClassifyRelation",
                column: "VideosId");

            migrationBuilder.CreateIndex(
                name: "IX_VodVideoComment_RootVideoCommentId",
                table: "VodVideoComment",
                column: "RootVideoCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_VodVideoComment_VideoId",
                table: "VodVideoComment",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VodVideoPart_VideoId",
                table: "VodVideoPart",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VodVideoClassifyRelation");

            migrationBuilder.DropTable(
                name: "VodVideoComment");

            migrationBuilder.DropTable(
                name: "VodVideoPart");

            migrationBuilder.DropTable(
                name: "VodVideoClassify");

            migrationBuilder.DropTable(
                name: "VodVideo");
        }
    }
}
