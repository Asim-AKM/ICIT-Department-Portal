using Domain_Service.Entities.AnnouncementAndDownload;
using Domain_Service.Entities.FacultyModule;
using Domain_Service.Entities.FYPPropsalModule;
using Domain_Service.Entities.StudentModule;
using Domain_Service.Entities.UserManagmentModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<Faculty> Faculties  { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<FYPTeam> FYPTeams { get; set; }    
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Announcement> Announcements { get; set; } 
        public DbSet<Project> Projects { get; set; }

    }
}
