using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class storegenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    StoreUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTag = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    YearOfEstablishment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    LegalName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsLocalOrOnlineStore = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    LogoMediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    RatingLikeId = table.Column<long>(type: "bigint", nullable: true),
                    StoreCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_MasterTags_StoreCategoryId",
                        column: x => x.StoreCategoryId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_MediaLibraries_LogoMediaLibraryId",
                        column: x => x.LogoMediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CountryId",
                table: "Stores",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_LogoMediaLibraryId",
                table: "Stores",
                column: "LogoMediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_RatingLikeId",
                table: "Stores",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StateId",
                table: "Stores",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreCategoryId",
                table: "Stores",
                column: "StoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_TenantId",
                table: "Stores",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
