using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS.Data.Migrations
{
    public partial class _23012018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organisation",
                table: "Candidate");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "OpeningSkills",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OpeningSkills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "OpeningSkills",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "OpeningSkills",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OpeningSkills",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OpeningSkills",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "OpeningSkills",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "OpeningSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Candidate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_OrganizationId",
                table: "Candidate",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_Organizations_OrganizationId",
                table: "Candidate",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_Organizations_OrganizationId",
                table: "Candidate");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_OrganizationId",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "OpeningSkills");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Candidate");

            migrationBuilder.AddColumn<string>(
                name: "Organisation",
                table: "Candidate",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
