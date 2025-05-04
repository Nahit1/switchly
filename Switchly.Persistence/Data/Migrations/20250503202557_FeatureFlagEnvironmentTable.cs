using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Switchly.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeatureFlagEnvironmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagId_Environment",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropColumn(
                name: "Environment",
                table: "FeatureFlagEnvironments");

            migrationBuilder.AddColumn<Guid>(
                name: "FeatureFlagEnvironmentId",
                table: "FeatureFlagEnvironments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FlagEnvironmentId",
                table: "FeatureFlagEnvironments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FlagEnvironments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagEnvironments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlagEnvironments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagEnvironmentId",
                table: "FeatureFlagEnvironments",
                column: "FeatureFlagEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagId",
                table: "FeatureFlagEnvironments",
                column: "FeatureFlagId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureFlagEnvironments_FlagEnvironmentId",
                table: "FeatureFlagEnvironments",
                column: "FlagEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FlagEnvironments_OrganizationId",
                table: "FlagEnvironments",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureFlagEnvironments_FeatureFlagEnvironments_FeatureFlag~",
                table: "FeatureFlagEnvironments",
                column: "FeatureFlagEnvironmentId",
                principalTable: "FeatureFlagEnvironments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureFlagEnvironments_FlagEnvironments_FlagEnvironmentId",
                table: "FeatureFlagEnvironments",
                column: "FlagEnvironmentId",
                principalTable: "FlagEnvironments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureFlagEnvironments_FeatureFlagEnvironments_FeatureFlag~",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureFlagEnvironments_FlagEnvironments_FlagEnvironmentId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropTable(
                name: "FlagEnvironments");

            migrationBuilder.DropIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagEnvironmentId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropIndex(
                name: "IX_FeatureFlagEnvironments_FlagEnvironmentId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropColumn(
                name: "FeatureFlagEnvironmentId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.DropColumn(
                name: "FlagEnvironmentId",
                table: "FeatureFlagEnvironments");

            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "FeatureFlagEnvironments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureFlagEnvironments_FeatureFlagId_Environment",
                table: "FeatureFlagEnvironments",
                columns: new[] { "FeatureFlagId", "Environment" },
                unique: true);
        }
    }
}
