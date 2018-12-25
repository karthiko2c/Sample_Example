using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _25042018_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ScheduleUserForCandidateId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ScheduleUserForCandidateId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ScheduleUserForCandidate",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ScheduleUserForCandidate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "ScheduleUserForCandidate",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ScheduleUserForCandidate",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ScheduleUserForCandidate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScheduleUserForCandidate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "ScheduleUserForCandidate",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ScheduleUserForCandidate",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ScheduleUserForCandidate",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUserForCandidate_UserId",
                table: "ScheduleUserForCandidate",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleUserForCandidate_Users_UserId",
                table: "ScheduleUserForCandidate",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleUserForCandidate_Users_UserId",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleUserForCandidate_UserId",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ScheduleUserForCandidate");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ScheduleUserForCandidate");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleUserForCandidateId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ScheduleUserForCandidateId",
                table: "Users",
                column: "ScheduleUserForCandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateId",
                table: "Users",
                column: "ScheduleUserForCandidateId",
                principalTable: "ScheduleUserForCandidate",
                principalColumn: "ScheduleUserForCandidateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
