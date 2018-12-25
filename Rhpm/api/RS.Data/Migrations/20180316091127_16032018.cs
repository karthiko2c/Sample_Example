using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _16032018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Openings");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ApprovalTransactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ApprovalTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Openings",
                nullable: false,
                defaultValue: false);
        }
    }
}
