using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "MB",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 2 );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "MB",
                table: "AspNetUsers");
        }
    }
}
