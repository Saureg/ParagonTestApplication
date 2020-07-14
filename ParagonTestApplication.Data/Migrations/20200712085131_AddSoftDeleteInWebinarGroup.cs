using Microsoft.EntityFrameworkCore.Migrations;

namespace ParagonTestApplication.Data.Migrations
{
    public partial class AddSoftDeleteInSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "IsDeleted",
                "Series",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "IsDeleted",
                "Series");
        }
    }
}