using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Switchly.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsEnabledFieldForFeatureFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "FeatureFlags",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "FeatureFlags");
        }
    }
}
