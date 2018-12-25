using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _16032018_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFurtherActionRequired",
                table: "ApprovalActions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFurtherActionRequired",
                table: "ApprovalTransactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFurtherActionRequired",
                table: "ApprovalTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFurtherActionRequired",
                table: "ApprovalActions",
                nullable: false,
                defaultValue: false);
        }
    }
}
