using Microsoft.EntityFrameworkCore.Migrations;

namespace ParagonTestApplication.Data.Migrations
{
    public partial class UniqueFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Series_Name",
                table: "Series",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Webinar_Name",
                table: "Webinar",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Series_Name",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Webinar_Name",
                table: "Webinar");
        }
    }
}
