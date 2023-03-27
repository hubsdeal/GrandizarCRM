using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreMapTableGenerated270323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreReviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReviewInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    RatingLikeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreReviews_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReviews_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReviews_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreZipCodeMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ZipCodeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreZipCodeMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreZipCodeMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreZipCodeMaps_ZipCodes_ZipCodeId",
                        column: x => x.ZipCodeId,
                        principalTable: "ZipCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreReviewFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    StoreReviewId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    RatingLikeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreReviewFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreReviewFeedbacks_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReviewFeedbacks_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreReviewFeedbacks_StoreReviews_StoreReviewId",
                        column: x => x.StoreReviewId,
                        principalTable: "StoreReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviewFeedbacks_ContactId",
                table: "StoreReviewFeedbacks",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviewFeedbacks_RatingLikeId",
                table: "StoreReviewFeedbacks",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviewFeedbacks_StoreReviewId",
                table: "StoreReviewFeedbacks",
                column: "StoreReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviewFeedbacks_TenantId",
                table: "StoreReviewFeedbacks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviews_ContactId",
                table: "StoreReviews",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviews_RatingLikeId",
                table: "StoreReviews",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviews_StoreId",
                table: "StoreReviews",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreReviews_TenantId",
                table: "StoreReviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreZipCodeMaps_StoreId",
                table: "StoreZipCodeMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreZipCodeMaps_TenantId",
                table: "StoreZipCodeMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreZipCodeMaps_ZipCodeId",
                table: "StoreZipCodeMaps",
                column: "ZipCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreReviewFeedbacks");

            migrationBuilder.DropTable(
                name: "StoreZipCodeMaps");

            migrationBuilder.DropTable(
                name: "StoreReviews");
        }
    }
}
