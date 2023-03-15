using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ZipCodeGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZipCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AreaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsianPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageHouseValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlackPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CBSA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CBSADiv = table.Column<string>(name: "CBSA_Div", type: "nvarchar(max)", nullable: true),
                    CBSADivName = table.Column<string>(name: "CBSA_Div_Name", type: "nvarchar(max)", nullable: true),
                    CBSAName = table.Column<string>(name: "CBSA_Name", type: "nvarchar(max)", nullable: true),
                    CBSAType = table.Column<string>(name: "CBSA_Type", type: "nvarchar(max)", nullable: true),
                    CSA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSAName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierRouteRateSortation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAliasCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAliasMixedCase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAliasName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityDeliveryIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityMixedCase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityStateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyANSI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyFIPS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountyMixedCase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayLightSaving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Elevation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FemalePopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinanceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HawaiianPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HispanicPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdsPerZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomePerHousehold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndianPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSAName = table.Column<string>(name: "MSA_Name", type: "nvarchar(max)", nullable: true),
                    MailingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MalePopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultiCounty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherPopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PMSA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PMSAName = table.Column<string>(name: "PMSA_Name", type: "nvarchar(max)", nullable: true),
                    PersonsPerHousehold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Population = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredLastLineKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryRecord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateANSI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateFIPS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueZIPName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhitePopulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CountyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZipCodes_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZipCodes_Counties_CountyId",
                        column: x => x.CountyId,
                        principalTable: "Counties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZipCodes_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZipCodes_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZipCodes_CityId",
                table: "ZipCodes",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ZipCodes_CountryId",
                table: "ZipCodes",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ZipCodes_CountyId",
                table: "ZipCodes",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_ZipCodes_StateId",
                table: "ZipCodes",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ZipCodes_TenantId",
                table: "ZipCodes",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZipCodes");
        }
    }
}
