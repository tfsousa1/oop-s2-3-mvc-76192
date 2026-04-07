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

        public DbSet<Branch> Branches { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Module> Modules { get; set; } = default!;
        public DbSet<StudentProfile> StudentProfiles { get; set; } = default!;
        public DbSet<FacultyProfile> FacultyProfiles { get; set; } = default!;
        public DbSet<FacultyCourseAssignment> FacultyCourseAssignments { get; set; } = default!;
        public DbSet<CourseEnrolment> CourseEnrolments { get; set; } = default!;
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = default!;
        public DbSet<Assignment> Assignments { get; set; } = default!;
        public DbSet<AssignmentResult> AssignmentResults { get; set; } = default!;
        public DbSet<Exam> Exams { get; set; } = default!;
        public DbSet<ExamResult> ExamResults { get; set; } = default!;

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

            builder.Entity<CourseEnrolment>()
                .HasOne(e => e.StudentProfile)
                .WithMany(s => s.Enrolments)
                .HasForeignKey(e => e.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourseEnrolment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrolments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AttendanceRecord>()
                .HasOne(a => a.CourseEnrolment)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(a => a.CourseEnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AssignmentResult>()
                .HasOne(ar => ar.Assignment)
                .WithMany(a => a.AssignmentResults)
                .HasForeignKey(ar => ar.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AssignmentResult>()
                .HasOne(ar => ar.StudentProfile)
                .WithMany(s => s.AssignmentResults)
                .HasForeignKey(ar => ar.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExamResult>()
                .HasOne(er => er.Exam)
                .WithMany(e => e.ExamResults)
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ExamResult>()
                .HasOne(er => er.StudentProfile)
                .WithMany(s => s.ExamResults)
                .HasForeignKey(er => er.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}