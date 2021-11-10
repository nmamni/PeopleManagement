using Microsoft.EntityFrameworkCore.Migrations;

namespace PM.Infrastructure.Migrations
{
    public partial class RelationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelationType",
                table: "PeopleRelations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationType",
                table: "PeopleRelations");
        }
    }
}
