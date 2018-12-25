using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _21022018_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateAssignedUser",
                columns: table => new
                {
                    CandidateAssignedUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApprovalEventId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateAssignedUser", x => x.CandidateAssignedUserId);
                    table.ForeignKey(
                        name: "FK_CandidateAssignedUser_ApprovalEvents_ApprovalEventId",
                        column: x => x.ApprovalEventId,
                        principalTable: "ApprovalEvents",
                        principalColumn: "ApprovalEventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateAssignedUser_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateAssignedUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssignedUser_ApprovalEventId",
                table: "CandidateAssignedUser",
                column: "ApprovalEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssignedUser_CandidateId",
                table: "CandidateAssignedUser",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssignedUser_UserId",
                table: "CandidateAssignedUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateAssignedUser");
        }
    }
}
