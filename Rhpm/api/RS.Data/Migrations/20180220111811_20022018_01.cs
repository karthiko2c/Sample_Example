using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _20022018_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Openings",
                newName: "IsApproved");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Candidates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Candidates");

            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "Openings",
                newName: "IsPublished");
        }
    }
}
