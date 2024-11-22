using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MB");

            migrationBuilder.CreateTable(
                name: "Author",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfBirth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookPoint",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<int>(type: "int", nullable: false),
                    Long = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthorsJoinTable",
                schema: "MB",
                columns: table => new
                {
                    AuthorBooksId = table.Column<int>(type: "int", nullable: false),
                    AuthorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthorsJoinTable", x => new { x.AuthorBooksId, x.AuthorsId });
                    table.ForeignKey(
                        name: "FK_BookAuthorsJoinTable_Author_AuthorsId",
                        column: x => x.AuthorsId,
                        principalSchema: "MB",
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthorsJoinTable_Book_AuthorBooksId",
                        column: x => x.AuthorBooksId,
                        principalSchema: "MB",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_BookAuthorsJoinTable_AuthorsId",
                schema: "MB",
                table: "BookAuthorsJoinTable",
                column: "AuthorsId");

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
                name: "BookAuthorsJoinTable",
                schema: "MB");

            migrationBuilder.DropTable(
                name: "UserBookItem",
                schema: "MB");

            migrationBuilder.DropTable(
                name: "Author",
                schema: "MB");

            migrationBuilder.DropTable(
                name: "BookPoint",
                schema: "MB");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "MB");
        }
    }
}
