using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.DAL.Migrations
{
    public partial class CreateCharacterWeaponRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CharacterWeaponRelations",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisingProfileRelations", x => new { x.CharacterId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_CharacterWeaponRelations_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterWeaponRelations_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWeaponRelations_CharacterId",
                table: "CharacterWeaponRelations",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWeaponRelations_WeaponId",
                table: "CharacterWeaponRelations",
                column: "WeaponId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterWeaponRelations"
            );
        }
    }
}
