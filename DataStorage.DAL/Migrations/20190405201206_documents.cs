using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataStorage.DAL.Migrations
{
    public partial class documents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProfile");

            migrationBuilder.AddColumn<int>(
                name: "StorageSize",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: false),
                    IsFile = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(nullable: false),
                    DocumentId1 = table.Column<Guid>(nullable: true),
                    EditAccess = table.Column<bool>(nullable: false),
                    WatchAccess = table.Column<bool>(nullable: false),
                    UserEntityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Documents_DocumentId1",
                        column: x => x.DocumentId1,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_AspNetUsers_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_OwnerId",
                table: "Documents",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_DocumentId1",
                table: "UserDocuments",
                column: "DocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UserEntityId",
                table: "UserDocuments",
                column: "UserEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDocuments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropColumn(
                name: "StorageSize",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ClientProfile",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProfile_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
