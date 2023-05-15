using Microsoft.EntityFrameworkCore.Migrations;

using SoftGrid.SeedData.StoredProc;


namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryHubIdaddedinUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PrimaryHubId",
                table: "AbpUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_PrimaryHubId",
                table: "AbpUsers",
                column: "PrimaryHubId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Hubs_PrimaryHubId",
                table: "AbpUsers",
                column: "PrimaryHubId",
                principalTable: "Hubs",
                principalColumn: "Id");

            SP_Seed.Up(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Hubs_PrimaryHubId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_PrimaryHubId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryHubId",
                table: "AbpUsers");
        }
    }
}
