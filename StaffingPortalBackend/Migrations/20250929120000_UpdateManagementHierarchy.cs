using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StaffingPortalBackend.Models;

#nullable disable

namespace StaffingPortalBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250929120000_UpdateManagementHierarchy")]
    public partial class UpdateManagementHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagementHierarchies_ManagementHierarchies_ManagerId",
                table: "ManagementHierarchies");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "ManagementHierarchies",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ManagementHierarchies_ManagerId",
                table: "ManagementHierarchies",
                newName: "IX_ManagementHierarchies_ParentId");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "ManagementHierarchies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "ManagementHierarchies",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql("DELETE FROM \"ManagementHierarchies\";");

            migrationBuilder.Sql(@"
                INSERT INTO ""ManagementHierarchies"" (""PersonId"", ""Level"")
                SELECT ""Id"", ""Level""
                FROM ""People"";
            ");

            migrationBuilder.Sql(@"
                UPDATE ""ManagementHierarchies"" mh
                SET ""ParentId"" = parentNode.""Id""
                FROM ""People"" p
                LEFT JOIN ""ManagementHierarchies"" parentNode
                    ON parentNode.""PersonId"" = (
                        CASE
                            WHEN p.""DivisionManagerId"" IS NOT NULL AND p.""DivisionManagerId"" <> p.""Id"" THEN p.""DivisionManagerId""
                            WHEN p.""TalentManagerId"" IS NOT NULL AND p.""TalentManagerId"" <> p.""Id"" THEN p.""TalentManagerId""
                            WHEN p.""HeadId"" IS NOT NULL AND p.""HeadId"" <> p.""Id"" THEN p.""HeadId""
                            WHEN p.""DeputyHeadId"" IS NOT NULL AND p.""DeputyHeadId"" <> p.""Id"" THEN p.""DeputyHeadId""
                            ELSE NULL
                        END
                    )
                WHERE mh.""PersonId"" = p.""Id"" AND parentNode.""Id"" IS NOT NULL;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "ManagementHierarchies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManagementHierarchies_PersonId",
                table: "ManagementHierarchies",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagementHierarchies_ManagementHierarchies_ParentId",
                table: "ManagementHierarchies",
                column: "ParentId",
                principalTable: "ManagementHierarchies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagementHierarchies_People_PersonId",
                table: "ManagementHierarchies",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagementHierarchies_ManagementHierarchies_ParentId",
                table: "ManagementHierarchies");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagementHierarchies_People_PersonId",
                table: "ManagementHierarchies");

            migrationBuilder.DropIndex(
                name: "IX_ManagementHierarchies_PersonId",
                table: "ManagementHierarchies");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "ManagementHierarchies");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ManagementHierarchies",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_ManagementHierarchies_ParentId",
                table: "ManagementHierarchies",
                newName: "IX_ManagementHierarchies_ManagerId");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "ManagementHierarchies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagementHierarchies_ManagementHierarchies_ManagerId",
                table: "ManagementHierarchies",
                column: "ManagerId",
                principalTable: "ManagementHierarchies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
