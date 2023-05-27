using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class CompletionPercentageaddedforTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TaskEvents",
                newName: "TaskOrEvent");

            migrationBuilder.AddColumn<int>(
                name: "CompletionPercentage",
                table: "TaskEvents",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionPercentage",
                table: "TaskEvents");

            migrationBuilder.RenameColumn(
                name: "TaskOrEvent",
                table: "TaskEvents",
                newName: "Status");
        }
    }
}
