using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.GroupManager.Postgres.Migrations
{
    public partial class asset_settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetSettingsProfileId",
                schema: "groups",
                table: "groups",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetSettingsProfileId",
                schema: "groups",
                table: "groups");
        }
    }
}
