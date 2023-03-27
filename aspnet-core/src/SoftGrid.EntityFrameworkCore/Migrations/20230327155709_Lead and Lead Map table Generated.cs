using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class LeadandLeadMaptableGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadPipelineStages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StageOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadPipelineStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadSources",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LeadScore = table.Column<int>(type: "int", nullable: true),
                    ExpectedSalesAmount = table.Column<double>(type: "float", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedClosingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    LeadSourceId = table.Column<long>(type: "bigint", nullable: true),
                    LeadPipelineStageId = table.Column<long>(type: "bigint", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Leads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leads_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_LeadPipelineStages_LeadPipelineStageId",
                        column: x => x.LeadPipelineStageId,
                        principalTable: "LeadPipelineStages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_LeadSources_LeadSourceId",
                        column: x => x.LeadSourceId,
                        principalTable: "LeadSources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leads_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfluenceScore = table.Column<int>(type: "int", nullable: true),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadContacts_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadNotes_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadPipelineStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExitDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnteredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    LeadPipelineStageId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadPipelineStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadPipelineStatuses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadPipelineStatuses_LeadPipelineStages_LeadPipelineStageId",
                        column: x => x.LeadPipelineStageId,
                        principalTable: "LeadPipelineStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadPipelineStatuses_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadReferralRewards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RewardType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RewardAmount = table.Column<double>(type: "float", nullable: true),
                    RewardStatus = table.Column<bool>(type: "bit", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadReferralRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadReferralRewards_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadReferralRewards_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadSalesTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadSalesTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadSalesTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadSalesTeams_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTag = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TagValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadTags_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadTags_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadTasks_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadTasks_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_ContactId",
                table: "LeadContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_LeadId",
                table: "LeadContacts",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadContacts_TenantId",
                table: "LeadContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadNotes_LeadId",
                table: "LeadNotes",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadNotes_TenantId",
                table: "LeadNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadPipelineStages_TenantId",
                table: "LeadPipelineStages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadPipelineStatuses_EmployeeId",
                table: "LeadPipelineStatuses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadPipelineStatuses_LeadId",
                table: "LeadPipelineStatuses",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadPipelineStatuses_LeadPipelineStageId",
                table: "LeadPipelineStatuses",
                column: "LeadPipelineStageId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadPipelineStatuses_TenantId",
                table: "LeadPipelineStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadReferralRewards_ContactId",
                table: "LeadReferralRewards",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadReferralRewards_LeadId",
                table: "LeadReferralRewards",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadReferralRewards_TenantId",
                table: "LeadReferralRewards",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_BusinessId",
                table: "Leads",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ContactId",
                table: "Leads",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_EmployeeId",
                table: "Leads",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_HubId",
                table: "Leads",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadPipelineStageId",
                table: "Leads",
                column: "LeadPipelineStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadSourceId",
                table: "Leads",
                column: "LeadSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductCategoryId",
                table: "Leads",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductId",
                table: "Leads",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_StoreId",
                table: "Leads",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TenantId",
                table: "Leads",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSalesTeams_EmployeeId",
                table: "LeadSalesTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSalesTeams_LeadId",
                table: "LeadSalesTeams",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSalesTeams_TenantId",
                table: "LeadSalesTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadSources_TenantId",
                table: "LeadSources",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTags_LeadId",
                table: "LeadTags",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTags_MasterTagCategoryId",
                table: "LeadTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTags_MasterTagId",
                table: "LeadTags",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTags_TenantId",
                table: "LeadTags",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTasks_LeadId",
                table: "LeadTasks",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTasks_TaskEventId",
                table: "LeadTasks",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadTasks_TenantId",
                table: "LeadTasks",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadContacts");

            migrationBuilder.DropTable(
                name: "LeadNotes");

            migrationBuilder.DropTable(
                name: "LeadPipelineStatuses");

            migrationBuilder.DropTable(
                name: "LeadReferralRewards");

            migrationBuilder.DropTable(
                name: "LeadSalesTeams");

            migrationBuilder.DropTable(
                name: "LeadTags");

            migrationBuilder.DropTable(
                name: "LeadTasks");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "LeadPipelineStages");

            migrationBuilder.DropTable(
                name: "LeadSources");
        }
    }
}
