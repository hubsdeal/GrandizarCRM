using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProjectofJobaddedinJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreAccountTeams_Employees_EmployeeId",
                table: "StoreAccountTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Stores_StoreId",
                table: "StoreTags");

            migrationBuilder.AlterColumn<long>(
                name: "StoreId",
                table: "StoreTags",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "StoreAccountTeams",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "ProjectOrJob",
                table: "Jobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreAccountTeams_Employees_EmployeeId",
                table: "StoreAccountTeams",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Stores_StoreId",
                table: "StoreTags",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreAccountTeams_Employees_EmployeeId",
                table: "StoreAccountTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Stores_StoreId",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "ProjectOrJob",
                table: "Jobs");

            migrationBuilder.AlterColumn<long>(
                name: "StoreId",
                table: "StoreTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "StoreAccountTeams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreAccountTeams_Employees_EmployeeId",
                table: "StoreAccountTeams",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Stores_StoreId",
                table: "StoreTags",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
