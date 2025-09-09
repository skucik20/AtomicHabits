using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class categoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AtomicHabits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryModelId",
                table: "AtomicHabits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtomicHabits_CategoryModelId",
                table: "AtomicHabits",
                column: "CategoryModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtomicHabits_CategoryModel_CategoryModelId",
                table: "AtomicHabits",
                column: "CategoryModelId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtomicHabits_CategoryModel_CategoryModelId",
                table: "AtomicHabits");

            migrationBuilder.DropTable(
                name: "CategoryModel");

            migrationBuilder.DropIndex(
                name: "IX_AtomicHabits_CategoryModelId",
                table: "AtomicHabits");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AtomicHabits");

            migrationBuilder.DropColumn(
                name: "CategoryModelId",
                table: "AtomicHabits");
        }
    }
}
