using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffingPortalBackend.Migrations
{
    public partial class AddJustificationToTitleHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "TitleHistories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Justification",
                table: "TitleHistories");
        }
    }
}
