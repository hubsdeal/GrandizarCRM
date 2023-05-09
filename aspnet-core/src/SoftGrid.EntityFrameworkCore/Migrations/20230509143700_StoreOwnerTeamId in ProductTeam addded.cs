using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreOwnerTeamIdinProductTeamaddded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StoreOwnerTeamId",
                table: "ProductTeams",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTeams_StoreOwnerTeamId",
                table: "ProductTeams",
                column: "StoreOwnerTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTeams_StoreOwnerTeams_StoreOwnerTeamId",
                table: "ProductTeams",
                column: "StoreOwnerTeamId",
                principalTable: "StoreOwnerTeams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTeams_StoreOwnerTeams_StoreOwnerTeamId",
                table: "ProductTeams");

            migrationBuilder.DropIndex(
                name: "IX_ProductTeams_StoreOwnerTeamId",
                table: "ProductTeams");

            migrationBuilder.DropColumn(
                name: "StoreOwnerTeamId",
                table: "ProductTeams");
        }
    }
}
