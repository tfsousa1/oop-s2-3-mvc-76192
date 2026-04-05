using AcmeGlobalCollege.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<FacultyProfile> FacultyProfiles { get; set; }
        public DbSet<FacultyCourseAssignment> FacultyCourseAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .HasOne(c => c.Branch)
                .WithMany(b => b.Courses)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Module>()
                .HasOne(m => m.Course)
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentProfile>()
                   .HasIndex(s => s.StudentNumber)
                   .IsUnique();

            builder.Entity<StudentProfile>()
                .HasOne(s => s.IdentityUser)
                .WithMany()
                .HasForeignKey(s => s.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FacultyProfile>()
                .HasOne(f => f.IdentityUser)
                .WithMany()
                .HasForeignKey(f => f.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FacultyCourseAssignment>()
                .HasOne(fca => fca.FacultyProfile)
                .WithMany(f => f.FacultyCourseAssignments)
                .HasForeignKey(fca => fca.FacultyProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FacultyCourseAssignment>()
                .HasOne(fca => fca.Course)
                .WithMany(c => c.FacultyAssignments)
                .HasForeignKey(fca => fca.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}