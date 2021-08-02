using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.DAL.Migrations
{
    public partial class CreateGamerWeaponRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamerWeaponRelations",
                columns: table => new
                {
                    GamerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamerWeaponRelations", x => new { x.GamerId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_GamerWeaponRelations_Gamer_GamerId",
                        column: x => x.GamerId,
                        principalTable: "Gamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamerWeaponRelations_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamerWeaponRelations_GamerId",
                table: "GamerWeaponRelations",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamerWeaponRelations_WeaponId",
                table: "GamerWeaponRelations",
                column: "WeaponId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamerWeaponRelations"
            );
        }
    }
}
