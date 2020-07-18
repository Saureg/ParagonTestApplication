using Microsoft.EntityFrameworkCore.Migrations;

namespace ParagonTestApplication.Data.Migrations
{
    // ReSharper disable once UnusedType.Global
    public partial class UniqueFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                "IX_Series_Name",
                "Series",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Webinar_Name",
                "Webinar",
                "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_Series_Name",
                "Series");

            migrationBuilder.DropIndex(
                "IX_Webinar_Name",
                "Webinar");
        }
    }
}