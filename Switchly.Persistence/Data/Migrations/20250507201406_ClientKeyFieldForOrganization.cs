using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Switchly.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientKeyFieldForOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientKey",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientKey",
                table: "Organizations");
        }
    }
}
