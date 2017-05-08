using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFSamurai.Data.Migrations
{
    public partial class QuotesAndHaircuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReallyDeep",
                table: "Quote");

            migrationBuilder.AddColumn<int>(
                name: "Haircut",
                table: "Samurais",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Quote",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Haircut",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Quote");

            migrationBuilder.AddColumn<bool>(
                name: "IsReallyDeep",
                table: "Quote",
                nullable: false,
                defaultValue: false);
        }
    }
}
