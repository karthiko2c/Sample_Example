using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _24042018_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleUserForCandidate",
                columns: table => new
                {
                    CandidateAssignedUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApprovalEventId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<Guid>(nullable: false),
                    IsFinished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleUserForCandidate", x => x.CandidateAssignedUserId);
                    table.ForeignKey(
                        name: "FK_ScheduleUserForCandidate_ApprovalEvents_ApprovalEventId",
                        column: x => x.ApprovalEventId,
                        principalTable: "ApprovalEvents",
                        principalColumn: "ApprovalEventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleUserForCandidate_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                column: "ScheduleUserForCandidateCandidateAssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUserForCandidate_ApprovalEventId",
                table: "ScheduleUserForCandidate",
                column: "ApprovalEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleUserForCandidate_CandidateId",
                table: "ScheduleUserForCandidate",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users",
                column: "ScheduleUserForCandidateCandidateAssignedUserId",
                principalTable: "ScheduleUserForCandidate",
                principalColumn: "CandidateAssignedUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ScheduleUserForCandidate_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ScheduleUserForCandidate");

            migrationBuilder.DropIndex(
                name: "IX_Users_ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ScheduleUserForCandidateCandidateAssignedUserId",
                table: "Users");
        }
    }
}
