using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class WidgetClassesgenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterWidgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    InternalDisplayNumber = table.Column<int>(type: "int", nullable: true),
                    ThumbnailImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterWidgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreMasterThemes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMasterThemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HubWidgetMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    WidgetTypeId = table.Column<int>(type: "int", nullable: false),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    MasterWidgetId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubWidgetMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubWidgetMaps_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubWidgetMaps_MasterWidgets_MasterWidgetId",
                        column: x => x.MasterWidgetId,
                        principalTable: "MasterWidgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreWidgetMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    WidgetTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MasterWidgetId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreWidgetMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreWidgetMaps_MasterWidgets_MasterWidgetId",
                        column: x => x.MasterWidgetId,
                        principalTable: "MasterWidgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreWidgetMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreThemeMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    StoreMasterThemeId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreThemeMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreThemeMaps_StoreMasterThemes_StoreMasterThemeId",
                        column: x => x.StoreMasterThemeId,
                        principalTable: "StoreMasterThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreThemeMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubWidgetContentMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    HubWidgetMapId = table.Column<long>(type: "bigint", nullable: false),
                    ContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubWidgetContentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubWidgetContentMaps_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubWidgetContentMaps_HubWidgetMaps_HubWidgetMapId",
                        column: x => x.HubWidgetMapId,
                        principalTable: "HubWidgetMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubWidgetProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    HubWidgetMapId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubWidgetProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubWidgetProductMaps_HubWidgetMaps_HubWidgetMapId",
                        column: x => x.HubWidgetMapId,
                        principalTable: "HubWidgetMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubWidgetProductMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubWidgetStoreMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    HubWidgetMapId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubWidgetStoreMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubWidgetStoreMaps_HubWidgetMaps_HubWidgetMapId",
                        column: x => x.HubWidgetMapId,
                        principalTable: "HubWidgetMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubWidgetStoreMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreWidgetContentMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreWidgetMapId = table.Column<long>(type: "bigint", nullable: false),
                    ContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreWidgetContentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreWidgetContentMaps_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreWidgetContentMaps_StoreWidgetMaps_StoreWidgetMapId",
                        column: x => x.StoreWidgetMapId,
                        principalTable: "StoreWidgetMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreWidgetProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreWidgetMapId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreWidgetProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreWidgetProductMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreWidgetProductMaps_StoreWidgetMaps_StoreWidgetMapId",
                        column: x => x.StoreWidgetMapId,
                        principalTable: "StoreWidgetMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetContentMaps_ContentId",
                table: "HubWidgetContentMaps",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetContentMaps_HubWidgetMapId",
                table: "HubWidgetContentMaps",
                column: "HubWidgetMapId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetContentMaps_TenantId",
                table: "HubWidgetContentMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetMaps_HubId",
                table: "HubWidgetMaps",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetMaps_MasterWidgetId",
                table: "HubWidgetMaps",
                column: "MasterWidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetMaps_TenantId",
                table: "HubWidgetMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetProductMaps_HubWidgetMapId",
                table: "HubWidgetProductMaps",
                column: "HubWidgetMapId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetProductMaps_ProductId",
                table: "HubWidgetProductMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetProductMaps_TenantId",
                table: "HubWidgetProductMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetStoreMaps_HubWidgetMapId",
                table: "HubWidgetStoreMaps",
                column: "HubWidgetMapId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetStoreMaps_StoreId",
                table: "HubWidgetStoreMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_HubWidgetStoreMaps_TenantId",
                table: "HubWidgetStoreMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterWidgets_TenantId",
                table: "MasterWidgets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMasterThemes_TenantId",
                table: "StoreMasterThemes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreThemeMaps_StoreId",
                table: "StoreThemeMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreThemeMaps_StoreMasterThemeId",
                table: "StoreThemeMaps",
                column: "StoreMasterThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreThemeMaps_TenantId",
                table: "StoreThemeMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetContentMaps_ContentId",
                table: "StoreWidgetContentMaps",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetContentMaps_StoreWidgetMapId",
                table: "StoreWidgetContentMaps",
                column: "StoreWidgetMapId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetContentMaps_TenantId",
                table: "StoreWidgetContentMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetMaps_MasterWidgetId",
                table: "StoreWidgetMaps",
                column: "MasterWidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetMaps_StoreId",
                table: "StoreWidgetMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetMaps_TenantId",
                table: "StoreWidgetMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetProductMaps_ProductId",
                table: "StoreWidgetProductMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetProductMaps_StoreWidgetMapId",
                table: "StoreWidgetProductMaps",
                column: "StoreWidgetMapId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWidgetProductMaps_TenantId",
                table: "StoreWidgetProductMaps",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubWidgetContentMaps");

            migrationBuilder.DropTable(
                name: "HubWidgetProductMaps");

            migrationBuilder.DropTable(
                name: "HubWidgetStoreMaps");

            migrationBuilder.DropTable(
                name: "StoreThemeMaps");

            migrationBuilder.DropTable(
                name: "StoreWidgetContentMaps");

            migrationBuilder.DropTable(
                name: "StoreWidgetProductMaps");

            migrationBuilder.DropTable(
                name: "HubWidgetMaps");

            migrationBuilder.DropTable(
                name: "StoreMasterThemes");

            migrationBuilder.DropTable(
                name: "StoreWidgetMaps");

            migrationBuilder.DropTable(
                name: "MasterWidgets");
        }
    }
}
