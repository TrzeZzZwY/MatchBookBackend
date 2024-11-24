using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class userLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLikesBooks",
                schema: "MB",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserBookItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikesBooks", x => new { x.UserId, x.UserBookItemId });
                    table.ForeignKey(
                        name: "FK_UserLikesBooks_UserBookItem_UserBookItemId",
                        column: x => x.UserBookItemId,
                        principalSchema: "MB",
                        principalTable: "UserBookItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikesBooks_UserBookItemId",
                schema: "MB",
                table: "UserLikesBooks",
                column: "UserBookItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikesBooks",
                schema: "MB");
        }
    }
}
