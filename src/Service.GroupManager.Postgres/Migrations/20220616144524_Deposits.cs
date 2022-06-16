using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.GroupManager.Postgres.Migrations
{
    public partial class Deposits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepositProfileId",
                schema: "groups",
                table: "groups",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositProfileId",
                schema: "groups",
                table: "groups");
        }
    }
}
