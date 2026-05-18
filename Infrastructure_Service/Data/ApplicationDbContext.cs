using Domain_Service.Entities.Academic;
using Domain_Service.Entities.Feedback;
using Domain_Service.Entities.Finance;
using Domain_Service.Entities.FYP;
using Domain_Service.Entities.Identity;
using Domain_Service.Entities.Shared;
using Infrastructure_Service.Data.SeedData.Infrastructure_Service.Data.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext) { }

        // Identity related DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }

        // Academic related DbSets
        public DbSet<Clerk> Clerks { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ResultLock> ResultLocks { get; set; }

        // Feedback related DbSets
        public DbSet<StudentFeedback> StudentFeedbacks { get; set; }

        // Finance related DbSets
        public DbSet<Challan> Challans { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<FeeRecord> FeeRecords { get; set; }

        //FYP related DbSets
        public DbSet<FYPProposal> FYPProposals { get; set; }
        public DbSet<ProposalTeamMember> ProposalTeamMembers { get; set; }
        public DbSet<FinalEvaluation> FinalEvaluations { get; set; }
        public DbSet<FYPMilestone> FYPMilestones { get; set; }
        public DbSet<SupervisionMeeting> SupervisionMeetings { get; set; }
        // Shared related DbSets

        public DbSet<Download> Downloads { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<BulkEnrollmentBatch> BulkEnrollmentBatches { get; set; }
        public DbSet<Notification> Notifications { get; set; }
       




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // SEED DATA
            // =========================
            var departments = DepartmentSeed.GetDepartments();
            modelBuilder.Entity<Department>().HasData(departments);

            // =========================
            // SUBJECT RELATIONSHIPS
            // =========================

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Department)
                .WithMany()
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Semester)
                .WithMany()
                .HasForeignKey(s => s.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // ENROLLMENT RELATIONSHIPS
            // =========================

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Subject)
                .WithMany()
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict); // important to avoid cascade loop

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Semester)
                .WithMany()
                .HasForeignKey(e => e.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // GRADE RELATIONSHIPS
            // =========================

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Enrollment)
                .WithMany()
                .HasForeignKey(g => g.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
