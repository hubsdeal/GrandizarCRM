using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class JobGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    FullTimeJobOrGigWorkProject = table.Column<bool>(type: "bit", nullable: false),
                    RemoteWorkOrOnSiteWork = table.Column<bool>(type: "bit", nullable: false),
                    SalaryBasedOrFixedPrice = table.Column<bool>(type: "bit", nullable: false),
                    SalaryOrStaffingRate = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ReferralPoints = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Template = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfJobs = table.Column<int>(type: "int", nullable: true),
                    MinimumExperience = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MaximumExperience = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobLocationFullAddress = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HireByDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternalJobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityLocation = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    JobStatusTypeId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_JobStatusTypes_JobStatusTypeId",
                        column: x => x.JobStatusTypeId,
                        principalTable: "JobStatusTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_BusinessId",
                table: "Jobs",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CityId",
                table: "Jobs",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CountryId",
                table: "Jobs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CurrencyId",
                table: "Jobs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobStatusTypeId",
                table: "Jobs",
                column: "JobStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_MasterTagCategoryId",
                table: "Jobs",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_MasterTagId",
                table: "Jobs",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProductCategoryId",
                table: "Jobs",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_StateId",
                table: "Jobs",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_StoreId",
                table: "Jobs",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TenantId",
                table: "Jobs",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
