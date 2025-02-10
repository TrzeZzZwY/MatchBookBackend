using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportingService.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MB");

            migrationBuilder.CreateTable(
                name: "CaseEntity",
                schema: "MB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReviewerId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CaseStatus = table.Column<int>(type: "int", nullable: false),
                    CaseItemType = table.Column<int>(type: "int", nullable: false),
                    SerializedCaseFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportType = table.Column<int>(type: "int", nullable: false),
                    ReportNote = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseEntity", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseEntity",
                schema: "MB");
        }
    }
}
