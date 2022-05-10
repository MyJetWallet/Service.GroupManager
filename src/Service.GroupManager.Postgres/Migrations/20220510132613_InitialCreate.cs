using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.GroupManager.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "groups");

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "groups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    ConverterProfileId = table.Column<string>(type: "text", nullable: true),
                    WithdrawalProfileId = table.Column<string>(type: "text", nullable: true),
                    InterestRateProfileId = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "groups",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ClientEmail = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.ClientId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profiles_GroupId",
                schema: "groups",
                table: "profiles",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "groups",
                schema: "groups");

            migrationBuilder.DropTable(
                name: "profiles",
                schema: "groups");
        }
    }
}
