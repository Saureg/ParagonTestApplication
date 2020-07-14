using Microsoft.EntityFrameworkCore.Migrations;

namespace ParagonTestApplication.Data.Migrations
{
    public partial class AddSoftDeleteInSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Series",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Series");
        }
    }
}
