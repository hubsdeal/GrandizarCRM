using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class BusinessMapTablesGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessAccountTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessAccountTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessAccountTeams_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessAccountTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessContactMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessContactMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessContactMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessContactMaps_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessJobMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessJobMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessJobMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessJobMaps_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_BusinessNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessNotes_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessProductMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessProductMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessStoreMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessStoreMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessStoreMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessStoreMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessTaskMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTaskMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessTaskMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessTaskMaps_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessUsers_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccountTeams_BusinessId",
                table: "BusinessAccountTeams",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccountTeams_EmployeeId",
                table: "BusinessAccountTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccountTeams_TenantId",
                table: "BusinessAccountTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessContactMaps_BusinessId",
                table: "BusinessContactMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessContactMaps_ContactId",
                table: "BusinessContactMaps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessContactMaps_TenantId",
                table: "BusinessContactMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessJobMaps_BusinessId",
                table: "BusinessJobMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessJobMaps_JobId",
                table: "BusinessJobMaps",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessJobMaps_TenantId",
                table: "BusinessJobMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessNotes_BusinessId",
                table: "BusinessNotes",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessNotes_TenantId",
                table: "BusinessNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProductMaps_BusinessId",
                table: "BusinessProductMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProductMaps_ProductId",
                table: "BusinessProductMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProductMaps_TenantId",
                table: "BusinessProductMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStoreMaps_BusinessId",
                table: "BusinessStoreMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStoreMaps_StoreId",
                table: "BusinessStoreMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessStoreMaps_TenantId",
                table: "BusinessStoreMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTaskMaps_BusinessId",
                table: "BusinessTaskMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTaskMaps_TaskEventId",
                table: "BusinessTaskMaps",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTaskMaps_TenantId",
                table: "BusinessTaskMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUsers_BusinessId",
                table: "BusinessUsers",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUsers_TenantId",
                table: "BusinessUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUsers_UserId",
                table: "BusinessUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessAccountTeams");

            migrationBuilder.DropTable(
                name: "BusinessContactMaps");

            migrationBuilder.DropTable(
                name: "BusinessJobMaps");

            migrationBuilder.DropTable(
                name: "BusinessNotes");

            migrationBuilder.DropTable(
                name: "BusinessProductMaps");

            migrationBuilder.DropTable(
                name: "BusinessStoreMaps");

            migrationBuilder.DropTable(
                name: "BusinessTaskMaps");

            migrationBuilder.DropTable(
                name: "BusinessUsers");
        }
    }
}
