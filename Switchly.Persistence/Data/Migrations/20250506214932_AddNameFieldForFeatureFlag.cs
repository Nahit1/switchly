using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Switchly.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNameFieldForFeatureFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FeatureFlags",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FeatureFlags");
        }
    }
}
