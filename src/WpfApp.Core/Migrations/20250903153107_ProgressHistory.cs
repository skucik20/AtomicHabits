using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class ProgressHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgressHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HabitCheckDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsHabitChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    AtomicHabitId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressHistory_AtomicHabits_AtomicHabitId",
                        column: x => x.AtomicHabitId,
                        principalTable: "AtomicHabits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressHistory_AtomicHabitId",
                table: "ProgressHistory",
                column: "AtomicHabitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgressHistory");
        }
    }
}
