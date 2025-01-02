using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBookItemImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemImageId",
                schema: "MB",
                table: "UserBookItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Image",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserBookItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookItem_ItemImageId",
                schema: "MB",
                table: "UserBookItem",
                column: "ItemImageId",
                unique: true,
                filter: "[ItemImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBookItem_Image_ItemImageId",
                schema: "MB",
                table: "UserBookItem",
                column: "ItemImageId",
                principalSchema: "MB",
                principalTable: "Image",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBookItem_Image_ItemImageId",
                schema: "MB",
                table: "UserBookItem");

            migrationBuilder.DropTable(
                name: "Image",
                schema: "MB");

            migrationBuilder.DropIndex(
                name: "IX_UserBookItem_ItemImageId",
                schema: "MB",
                table: "UserBookItem");

            migrationBuilder.DropColumn(
                name: "ItemImageId",
                schema: "MB",
                table: "UserBookItem");
        }
    }
}
