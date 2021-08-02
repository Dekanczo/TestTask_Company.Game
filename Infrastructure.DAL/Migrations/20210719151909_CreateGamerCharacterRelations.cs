using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.DAL.Migrations
{
    public partial class CreateGamerCharacterRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "GamerCharacterRelations",
                columns: table => new
                {
                    GamerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamerCharacterRelations", x => new { x.GamerId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_GamerCharacterRelations_Gamer_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Gamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamerCharacterRelations_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamerCharacterRelations_GamerId",
                table: "GamerCharacterRelations",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamerCharacterRelations_CharacterId",
                table: "GamerCharacterRelations",
                column: "CharacterId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamerCharacterRelations"
            );
        }
    }
}
