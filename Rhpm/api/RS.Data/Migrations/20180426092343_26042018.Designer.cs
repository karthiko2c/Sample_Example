﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RS.Common.Enums;
using RS.Data;
using System;

namespace RS.Data.Migrations
{
    [DbContext(typeof(RSContext))]
    [Migration("20180426092343_26042018")]
    partial class _26042018
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RS.Entity.Models.ApprovalActions", b =>
                {
                    b.Property<int>("ApprovalActionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApprovalActionName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("ApprovalEventId");

                    b.HasKey("ApprovalActionId");

                    b.HasIndex("ApprovalEventId");

                    b.ToTable("ApprovalActions");
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalEventRoles", b =>
                {
                    b.Property<int>("ApprovalEventRoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApprovalEventId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("RoleId");

                    b.Property<Guid>("UserId");

                    b.HasKey("ApprovalEventRoleId");

                    b.HasIndex("ApprovalEventId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("ApprovalEventRoles");
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalEvents", b =>
                {
                    b.Property<int>("ApprovalEventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApprovalEventName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("ApprovalEventOrder");

                    b.Property<int>("ApprovalId");

                    b.HasKey("ApprovalEventId");

                    b.HasIndex("ApprovalId");

                    b.ToTable("ApprovalEvents");
                });

            modelBuilder.Entity("RS.Entity.Models.Approvals", b =>
                {
                    b.Property<int>("ApprovalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApprovalDesc")
                        .HasMaxLength(500);

                    b.Property<string>("ApprovalName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("ApprovalId");

                    b.ToTable("Approvals");
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalTransactionDetails", b =>
                {
                    b.Property<int>("ApprovalTransactionDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApprovalActionId");

                    b.Property<int>("ApprovalTransactionId");

                    b.Property<string>("Comments")
                        .HasMaxLength(500);

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<int>("EventOrderNumber");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.HasKey("ApprovalTransactionDetailId");

                    b.HasIndex("ApprovalTransactionId");

                    b.ToTable("ApprovalTransactionDetails");
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalTransactions", b =>
                {
                    b.Property<int>("ApprovalTransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApprovalActionId");

                    b.Property<int>("ApprovalId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<Guid>("EntityId");

                    b.Property<int>("EntityType");

                    b.Property<int>("EventOrderNumber");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFurtherActionRequired");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("NextEventOrderNumber");

                    b.HasKey("ApprovalTransactionId");

                    b.HasIndex("ApprovalActionId");

                    b.ToTable("ApprovalTransactions");
                });

            modelBuilder.Entity("RS.Entity.Models.CandidateAssignedUser", b =>
                {
                    b.Property<int>("CandidateAssignedUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApprovalEventId");

                    b.Property<Guid>("CandidateId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<Guid>("UserId");

                    b.HasKey("CandidateAssignedUserId");

                    b.HasIndex("ApprovalEventId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("UserId");

                    b.ToTable("CandidateAssignedUser");
                });

            modelBuilder.Entity("RS.Entity.Models.CandidateDocuments", b =>
                {
                    b.Property<Guid>("CandidateDocumentId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CandidateId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("UploadedDocument")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("CandidateDocumentId");

                    b.HasIndex("CandidateId");

                    b.ToTable("CandidateDocuments");
                });

            modelBuilder.Entity("RS.Entity.Models.Candidates", b =>
                {
                    b.Property<Guid>("CandidateId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<int>("ExperienceMonth");

                    b.Property<int>("ExperienceYear");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Gender");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsReadyForInterview");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("OrganizationId");

                    b.Property<int>("QualificationId");

                    b.HasKey("CandidateId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("QualificationId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("RS.Entity.Models.OpeningCandidates", b =>
                {
                    b.Property<int>("CandidateOpeningId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CandidateId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<Guid>("OpeningId");

                    b.HasKey("CandidateOpeningId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("OpeningId");

                    b.ToTable("OpeningCandidates");
                });

            modelBuilder.Entity("RS.Entity.Models.Openings", b =>
                {
                    b.Property<Guid>("OpeningId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("OpeningId");

                    b.ToTable("Openings");
                });

            modelBuilder.Entity("RS.Entity.Models.OpeningSkills", b =>
                {
                    b.Property<int>("OpeningSkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<Guid>("OpeningId");

                    b.Property<int>("SkillId");

                    b.Property<int>("SkillType");

                    b.HasKey("OpeningSkillId");

                    b.HasIndex("OpeningId");

                    b.HasIndex("SkillId");

                    b.ToTable("OpeningSkills");
                });

            modelBuilder.Entity("RS.Entity.Models.Organizations", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("RS.Entity.Models.Qualifications", b =>
                {
                    b.Property<int>("QualificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("QualificationId");

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("RS.Entity.Models.Roles", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RS.Entity.Models.ScheduleUserForCandidate", b =>
                {
                    b.Property<int>("ScheduleUserForCandidateId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApprovalEventId");

                    b.Property<Guid>("CandidateId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFinished");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<DateTime>("ScheduledOn");

                    b.Property<Guid>("UserId");

                    b.HasKey("ScheduleUserForCandidateId");

                    b.HasIndex("ApprovalEventId");

                    b.HasIndex("CandidateId");

                    b.HasIndex("UserId");

                    b.ToTable("ScheduleUserForCandidate");
                });

            modelBuilder.Entity("RS.Entity.Models.Skills", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("RS.Entity.Models.UserRoles", b =>
                {
                    b.Property<Guid>("UserRolesId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("RoleId");

                    b.Property<Guid>("UserId");

                    b.HasKey("UserRolesId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RS.Entity.Models.Users", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<Guid?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalActions", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalEvents", "ApprovalEvent")
                        .WithMany("ApprovalActions")
                        .HasForeignKey("ApprovalEventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalEventRoles", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalEvents", "ApprovalEvent")
                        .WithMany("ApprovalEventRoles")
                        .HasForeignKey("ApprovalEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Roles", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalEvents", b =>
                {
                    b.HasOne("RS.Entity.Models.Approvals", "Approval")
                        .WithMany()
                        .HasForeignKey("ApprovalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalTransactionDetails", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalTransactions")
                        .WithMany("ApprovalTransactionDetails")
                        .HasForeignKey("ApprovalTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.ApprovalTransactions", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalActions", "ApprovalAction")
                        .WithMany()
                        .HasForeignKey("ApprovalActionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.CandidateAssignedUser", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalEvents", "ApprovalEvent")
                        .WithMany()
                        .HasForeignKey("ApprovalEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Candidates", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.CandidateDocuments", b =>
                {
                    b.HasOne("RS.Entity.Models.Candidates", "Candidate")
                        .WithMany("CandidateDocuments")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.Candidates", b =>
                {
                    b.HasOne("RS.Entity.Models.Organizations", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Qualifications", "Qualification")
                        .WithMany("Candidate")
                        .HasForeignKey("QualificationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.OpeningCandidates", b =>
                {
                    b.HasOne("RS.Entity.Models.Candidates", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Openings", "Opening")
                        .WithMany()
                        .HasForeignKey("OpeningId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.OpeningSkills", b =>
                {
                    b.HasOne("RS.Entity.Models.Openings", "Opening")
                        .WithMany("OpeningSkills")
                        .HasForeignKey("OpeningId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Skills", "Skill")
                        .WithMany("OpeningSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.ScheduleUserForCandidate", b =>
                {
                    b.HasOne("RS.Entity.Models.ApprovalEvents", "ApprovalEvent")
                        .WithMany()
                        .HasForeignKey("ApprovalEventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Candidates", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RS.Entity.Models.UserRoles", b =>
                {
                    b.HasOne("RS.Entity.Models.Roles", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RS.Entity.Models.Users", "user")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
