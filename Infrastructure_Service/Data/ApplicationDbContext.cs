using Domain_Service.Entities.Academic;
using Domain_Service.Entities.Finance;
using Domain_Service.Entities.FYP;
using Domain_Service.Entities.Identity;
using Domain_Service.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext){ }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<FeeRecord> FeeRecords { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Faculty> Faculties  { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ProposalTeamMember> ProposalTeamMembers { get; set; }    
        public DbSet<FYPProposal> FYPProposals { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Announcement> Announcements { get; set; } 

    }
}
