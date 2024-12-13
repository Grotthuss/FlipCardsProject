using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlipCardProject.Migrations
{
    /// <inheritdoc />
    public partial class Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flipcard");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Salt");

            migrationBuilder.CreateTable(
                name: "Flipcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Concept = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Mnemonic = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flipcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flipcards_FlipCardSets_SetId",
                        column: x => x.SetId,
                        principalTable: "FlipCardSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flipcards_SetId",
                table: "Flipcards",
                column: "SetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flipcards");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "Users",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "Flipcard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Concept = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Mnemonic = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Question = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flipcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flipcard_FlipCardSets_SetId",
                        column: x => x.SetId,
                        principalTable: "FlipCardSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flipcard_SetId",
                table: "Flipcard",
                column: "SetId");
        }
    }
}
