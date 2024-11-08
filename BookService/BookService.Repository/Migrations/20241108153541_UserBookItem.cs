using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UserBookItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBookItem",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookReferenceId = table.Column<int>(type: "int", nullable: false),
                    BookPointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBookItem_BookPoint_BookPointId",
                        column: x => x.BookPointId,
                        principalSchema: "MB",
                        principalTable: "BookPoint",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBookItem_Book_BookReferenceId",
                        column: x => x.BookReferenceId,
                        principalSchema: "MB",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookItem_BookPointId",
                schema: "MB",
                table: "UserBookItem",
                column: "BookPointId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookItem_BookReferenceId",
                schema: "MB",
                table: "UserBookItem",
                column: "BookReferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBookItem",
                schema: "MB");
        }
    }
}
