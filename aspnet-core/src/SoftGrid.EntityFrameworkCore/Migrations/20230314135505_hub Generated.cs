using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class hubGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedPopulation = table.Column<int>(type: "int", nullable: true),
                    HasParentHub = table.Column<bool>(type: "bit", nullable: false),
                    ParentHubId = table.Column<long>(type: "bigint", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Live = table.Column<bool>(type: "bit", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OfficeFullAddress = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    PartnerOrOwned = table.Column<bool>(type: "bit", nullable: false),
                    PictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearlyRevenue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CountyId = table.Column<long>(type: "bigint", nullable: true),
                    HubTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Hubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hubs_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hubs_Counties_CountyId",
                        column: x => x.CountyId,
                        principalTable: "Counties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hubs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hubs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hubs_HubTypes_HubTypeId",
                        column: x => x.HubTypeId,
                        principalTable: "HubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hubs_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_CityId",
                table: "Hubs",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_CountryId",
                table: "Hubs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_CountyId",
                table: "Hubs",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_CurrencyId",
                table: "Hubs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_HubTypeId",
                table: "Hubs",
                column: "HubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_StateId",
                table: "Hubs",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_TenantId",
                table: "Hubs",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hubs");
        }
    }
}
