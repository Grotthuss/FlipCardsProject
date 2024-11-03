using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlipCardProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sets",
                columns: table => new
                {
                    SetName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sets", x => x.SetName);
                });

            migrationBuilder.CreateTable(
                name: "Flipcard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Concept = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mnemonic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerSetName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flipcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flipcard_sets_OwnerSetName",
                        column: x => x.OwnerSetName,
                        principalTable: "sets",
                        principalColumn: "SetName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flipcard_OwnerSetName",
                table: "Flipcard",
                column: "OwnerSetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flipcard");

            migrationBuilder.DropTable(
                name: "sets");
        }
    }
}
