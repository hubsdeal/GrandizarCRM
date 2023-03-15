using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class contactReGeneratedandcompanygenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Contacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    YearOfEstablishment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LocationTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(2056)", maxLength: 2056, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EinTaxId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Logo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InternalRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Businesses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Businesses_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_CityId",
                table: "Businesses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_CountryId",
                table: "Businesses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_StateId",
                table: "Businesses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_TenantId",
                table: "Businesses",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Contacts");
        }
    }
}
