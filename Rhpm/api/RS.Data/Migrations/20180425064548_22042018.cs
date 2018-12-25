using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _22042018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                newName: "ScheduleUserForCandidateId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                newName: "IX_Users_ScheduleUserForCandidateId");

            migrationBuilder.RenameColumn(
                name: "CandidateAssignedUserId",
                table: "ScheduleUserForCandidate",
                newName: "ScheduleUserForCandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateId",
                table: "Users",
                column: "ScheduleUserForCandidateId",
                principalTable: "ScheduleUserForCandidate",
                principalColumn: "ScheduleUserForCandidateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ScheduleUserForCandidateId",
                table: "Users",
                newName: "ScheduleUserForCandidateCandidateAssignedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ScheduleUserForCandidateId",
                table: "Users",
                newName: "IX_Users_ScheduleUserForCandidateCandidateAssignedUserId");

            migrationBuilder.RenameColumn(
                name: "ScheduleUserForCandidateId",
                table: "ScheduleUserForCandidate",
                newName: "CandidateAssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                column: "ScheduleUserForCandidateCandidateAssignedUserId",
                principalTable: "ScheduleUserForCandidate",
                principalColumn: "CandidateAssignedUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
