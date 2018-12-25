using Microsoft.EntityFrameworkCore;
using RS.Entity;
using RS.Entity.Models;

namespace RS.Data
{
    public class RSContext : DbContext
    {
        public RSContext(DbContextOptions<RSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ApprovalActions> ApprovalActions { get; set; }
        public virtual DbSet<ApprovalEventRoles> ApprovalEventRoles { get; set; }
        public virtual DbSet<ApprovalEvents> ApprovalEvents { get; set; }
        public virtual DbSet<Approvals> Approvals { get; set; }
        public virtual DbSet<ApprovalTransactionDetails> ApprovalTransactionDetails { get; set; }
        public virtual DbSet<ApprovalTransactions> ApprovalTransactions { get; set; }
        public virtual DbSet<Candidates> Candidates { get; set; }
        public virtual DbSet<CandidateDocuments> CandidateDocuments { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<Openings> Openings { get; set; }
        public virtual DbSet<OpeningSkills> OpeningSkills { get; set; }
        public virtual DbSet<OpeningCandidates> OpeningCandidates { get; set; }
        public virtual DbSet<Qualifications> Qualifications { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<CandidateAssignedUser> CandidateAssignedUser { get; set; }
        public virtual DbSet<ScheduleUserForCandidate> ScheduleUserForCandidate { get; set; }
    }
}
