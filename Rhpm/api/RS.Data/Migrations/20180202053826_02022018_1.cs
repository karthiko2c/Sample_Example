using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _02022018_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ApprovalEventRoles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalEventRoles_UserId",
                table: "ApprovalEventRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalEventRoles_Users_UserId",
                table: "ApprovalEventRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalEventRoles_Users_UserId",
                table: "ApprovalEventRoles");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalEventRoles_UserId",
                table: "ApprovalEventRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApprovalEventRoles");
        }
    }
}
