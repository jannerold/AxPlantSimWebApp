using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxPlantSimWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkplaceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkplaceId = table.Column<string>(type: "TEXT", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Week1 = table.Column<string>(type: "TEXT", nullable: false),
                    Week2 = table.Column<string>(type: "TEXT", nullable: false),
                    Week3 = table.Column<string>(type: "TEXT", nullable: false),
                    Week4 = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    WorkplaceList = table.Column<string>(type: "TEXT", nullable: false),
                    WorkerList = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workplaces");
        }
    }
}
