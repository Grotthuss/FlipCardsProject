using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlipCardProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Flipcard");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Flipcard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
