using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StaffingPortalBackend.Migrations
{
    public partial class InitialPostgreMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagementHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementHierarchies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementHierarchies_ManagementHierarchies_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "ManagementHierarchies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TechStack = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    Search = table.Column<string>(type: "text", nullable: false),
                    Signed = table.Column<bool>(type: "boolean", nullable: false),
                    PositionSigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionClosed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Stream = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    Attachment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuarterlyDepartmentSalaryChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Q1 = table.Column<decimal>(type: "numeric", nullable: false),
                    Q2 = table.Column<decimal>(type: "numeric", nullable: false),
                    Q3 = table.Column<decimal>(type: "numeric", nullable: false),
                    Q4 = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyDepartmentSalaryChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: true),
                    Stream = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    IsDismissed = table.Column<bool>(type: "boolean", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AvailableFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    TechStack = table.Column<string>(type: "text", nullable: false),
                    Stream = table.Column<string>(type: "text", nullable: false),
                    TMAware = table.Column<string>(type: "text", nullable: false),
                    PlannedAssignment = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<string>(type: "text", nullable: false),
                    AssignmentExistsInGCP = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    DivisionManagerId = table.Column<int>(type: "integer", nullable: true),
                    TalentManagerId = table.Column<int>(type: "integer", nullable: true),
                    HeadId = table.Column<int>(type: "integer", nullable: true),
                    DeputyHeadId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_People_DeputyHeadId",
                        column: x => x.DeputyHeadId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_People_DivisionManagerId",
                        column: x => x.DivisionManagerId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_People_HeadId",
                        column: x => x.HeadId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_People_TalentManagerId",
                        column: x => x.TalentManagerId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectCandidates",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCandidates", x => new { x.ProjectId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ProjectCandidates_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCandidates_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryReviewHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SalaryReviewId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: false),
                    OldSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProposedSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ContractType = table.Column<string>(type: "text", nullable: false),
                    NewSalaryStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SalaryReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DivisionManagerComments = table.Column<string>(type: "text", nullable: false),
                    TalentManagerComments = table.Column<string>(type: "text", nullable: false),
                    ReviewResult = table.Column<string>(type: "text", nullable: false),
                    ArchivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryReviewHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_SalaryReviewHistories_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SalaryReviewRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewSalaryStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SalaryReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TalentManagerComments = table.Column<string>(type: "text", nullable: false),
                    DivisionManagerComments = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ContractType = table.Column<string>(type: "text", nullable: false),
                    OldSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NewSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedByDM = table.Column<bool>(type: "boolean", nullable: true),
                    ApprovedByHead = table.Column<bool>(type: "boolean", nullable: true),
                    ApprovedByVP = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryReviewRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryReviewRequests_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: false),
                    OldSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ProposedSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ContractType = table.Column<string>(type: "text", nullable: false),
                    NewSalaryStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SalaryReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DivisionManagerComments = table.Column<string>(type: "text", nullable: false),
                    TalentManagerComments = table.Column<string>(type: "text", nullable: false),
                    ReviewResult = table.Column<string>(type: "text", nullable: false),
                    ApprovedByDM = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryReviews_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagementHierarchies_ManagerId",
                table: "ManagementHierarchies",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_People_DeputyHeadId",
                table: "People",
                column: "DeputyHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_People_DivisionManagerId",
                table: "People",
                column: "DivisionManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_People_HeadId",
                table: "People",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_People_TalentManagerId",
                table: "People",
                column: "TalentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_People_UserId",
                table: "People",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCandidates_PersonId",
                table: "ProjectCandidates",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryReviewHistories_PersonId",
                table: "SalaryReviewHistories",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryReviewRequests_PersonId",
                table: "SalaryReviewRequests",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryReviews_PersonId",
                table: "SalaryReviews",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagementHierarchies");

            migrationBuilder.DropTable(
                name: "ProjectCandidates");

            migrationBuilder.DropTable(
                name: "QuarterlyDepartmentSalaryChanges");

            migrationBuilder.DropTable(
                name: "SalaryReviewHistories");

            migrationBuilder.DropTable(
                name: "SalaryReviewRequests");

            migrationBuilder.DropTable(
                name: "SalaryReviews");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
