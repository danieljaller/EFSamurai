using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFSamurai.Data.Migrations
{
    public partial class LogsAndEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattleLogId",
                table: "Battles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BattleLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BattleLogId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleEvent_BattleLog_BattleLogId",
                        column: x => x.BattleLogId,
                        principalTable: "BattleLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battles_BattleLogId",
                table: "Battles",
                column: "BattleLogId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleEvent_BattleLogId",
                table: "BattleEvent",
                column: "BattleLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battles_BattleLog_BattleLogId",
                table: "Battles",
                column: "BattleLogId",
                principalTable: "BattleLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battles_BattleLog_BattleLogId",
                table: "Battles");

            migrationBuilder.DropTable(
                name: "BattleEvent");

            migrationBuilder.DropTable(
                name: "BattleLog");

            migrationBuilder.DropIndex(
                name: "IX_Battles_BattleLogId",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "BattleLogId",
                table: "Battles");
        }
    }
}
