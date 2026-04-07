using AcmeGlobalCollege.Web.Data;
using AcmeGlobalCollege.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcmeGlobalCollege.Web.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedAcademicDataAsync(context, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Faculty", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to create role '{role}': {errors}");
                    }
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(userManager, "admin@acmeglobal.ie", "AcmeVGC!26", "Admin");

            await CreateUserIfNotExistsAsync(userManager, "albus.dumbledore@acmeglobal.ie", "Dumbledore!26", "Faculty");
            await CreateUserIfNotExistsAsync(userManager, "severus.snape@acmeglobal.ie", "Snape!26", "Faculty");
            await CreateUserIfNotExistsAsync(userManager, "minerva.mcgonagall@acmeglobal.ie", "McGonagall!26", "Faculty");

            await CreateUserIfNotExistsAsync(userManager, "hermione.granger@acmeglobal.ie", "Hermione!26", "Student");
            await CreateUserIfNotExistsAsync(userManager, "harry.potter@acmeglobal.ie", "Harry!26", "Student");

            // FIX: Luna!26 tinha 7 caracteres e falhava na policy
            await CreateUserIfNotExistsAsync(userManager, "luna.lovegood@acmeglobal.ie", "LunaLove!26", "Student");
        }

        private static async Task CreateUserIfNotExistsAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(existingUser, role);

                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to add role '{role}' to '{email}': {errors}");
                    }
                }

                return;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user '{email}': {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to add role '{role}' to '{email}': {errors}");
            }
        }

        private static async Task SeedAcademicDataAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            // BRANCHES
            if (!await context.Branches.AnyAsync())
            {
                context.Branches.AddRange(
                    new Branch
                    {
                        Name = "Dublin City Branch",
                        Address = "15 College Green, Dublin 2"
                    },
                    new Branch
                    {
                        Name = "Tallaght Branch",
                        Address = "Blessington Road, Tallaght, Dublin 24"
                    },
                    new Branch
                    {
                        Name = "Blanchardstown Branch",
                        Address = "Main Street, Blanchardstown, Dublin 15"
                    }
                );

                await context.SaveChangesAsync();
            }

            var dublinBranch = await context.Branches.FirstAsync(b => b.Name == "Dublin City Branch");
            var tallaghtBranch = await context.Branches.FirstAsync(b => b.Name == "Tallaght Branch");
            var blanchardstownBranch = await context.Branches.FirstAsync(b => b.Name == "Blanchardstown Branch");

            // COURSES
            if (!await context.Courses.AnyAsync())
            {
                context.Courses.AddRange(
                    new Course
                    {
                        Name = "BSc Computer Science",
                        BranchId = dublinBranch.Id,
                        StartDate = new DateTime(2026, 9, 1),
                        EndDate = new DateTime(2030, 5, 31)
                    },
                    new Course
                    {
                        Name = "BSc Pharmaceutical and Chemical Science",
                        BranchId = tallaghtBranch.Id,
                        StartDate = new DateTime(2026, 9, 1),
                        EndDate = new DateTime(2030, 5, 31)
                    },
                    new Course
                    {
                        Name = "BSc Biological Sciences",
                        BranchId = blanchardstownBranch.Id,
                        StartDate = new DateTime(2026, 9, 1),
                        EndDate = new DateTime(2030, 5, 31)
                    },
                    new Course
                    {
                        Name = "BSc Mechanical and Manufacturing Engineering",
                        BranchId = tallaghtBranch.Id,
                        StartDate = new DateTime(2026, 9, 1),
                        EndDate = new DateTime(2030, 5, 31)
                    }
                );

                await context.SaveChangesAsync();
            }

            var computerScience = await context.Courses.FirstAsync(c => c.Name == "BSc Computer Science");
            var pharmaceuticalScience = await context.Courses.FirstAsync(c => c.Name == "BSc Pharmaceutical and Chemical Science");
            var biologicalSciences = await context.Courses.FirstAsync(c => c.Name == "BSc Biological Sciences");
            var engineering = await context.Courses.FirstAsync(c => c.Name == "BSc Mechanical and Manufacturing Engineering");

            // MODULES
            if (!await context.Modules.AnyAsync())
            {
                context.Modules.AddRange(
                    new Module
                    {
                        Name = "Cybersecurity Fundamentals",
                        Description = "Introduction to cybersecurity principles and threat awareness.",
                        CourseId = computerScience.Id
                    },
                    new Module
                    {
                        Name = "Algorithms and Data Structures",
                        Description = "Core computational problem-solving and data organization.",
                        CourseId = computerScience.Id
                    },
                    new Module
                    {
                        Name = "Web Application Development",
                        Description = "Design and development of modern web applications.",
                        CourseId = computerScience.Id
                    },
                    new Module
                    {
                        Name = "Human-Computer Interaction",
                        Description = "User-centred design and usability principles.",
                        CourseId = computerScience.Id
                    },

                    new Module
                    {
                        Name = "Organic and Medicinal Chemistry",
                        Description = "Chemical structure, reactions and pharmaceutical applications.",
                        CourseId = pharmaceuticalScience.Id
                    },
                    new Module
                    {
                        Name = "Analytical Chemistry Laboratory",
                        Description = "Laboratory methods for chemical testing and analysis.",
                        CourseId = pharmaceuticalScience.Id
                    },
                    new Module
                    {
                        Name = "Pharmacology and Toxicology",
                        Description = "Drug action, dosage and toxicological response.",
                        CourseId = pharmaceuticalScience.Id
                    },
                    new Module
                    {
                        Name = "Plant-Derived Bioactive Compounds",
                        Description = "Study of plant-based compounds in health science.",
                        CourseId = pharmaceuticalScience.Id
                    },

                    new Module
                    {
                        Name = "Zoology and Animal Behaviour",
                        Description = "Animal biology, classification and behavioural studies.",
                        CourseId = biologicalSciences.Id
                    },
                    new Module
                    {
                        Name = "Plant Biology and Physiology",
                        Description = "Structure and function of plant systems.",
                        CourseId = biologicalSciences.Id
                    },
                    new Module
                    {
                        Name = "Ecology and Field Studies",
                        Description = "Environmental systems and biological field observation.",
                        CourseId = biologicalSciences.Id
                    },
                    new Module
                    {
                        Name = "Genetics and Evolution",
                        Description = "Inheritance, variation and evolutionary mechanisms.",
                        CourseId = biologicalSciences.Id
                    },

                    new Module
                    {
                        Name = "Mechanics of Materials",
                        Description = "Mechanical behaviour of materials under load.",
                        CourseId = engineering.Id
                    },
                    new Module
                    {
                        Name = "Mechanical Design",
                        Description = "Engineering design principles and applied problem-solving.",
                        CourseId = engineering.Id
                    },
                    new Module
                    {
                        Name = "Energy Systems and Power Transfer",
                        Description = "Energy conversion and transmission in engineering systems.",
                        CourseId = engineering.Id
                    },
                    new Module
                    {
                        Name = "Manufacturing Processes",
                        Description = "Methods and technologies used in manufacturing industries.",
                        CourseId = engineering.Id
                    }
                );

                await context.SaveChangesAsync();
            }

            // USERS
            var dumbledoreUser = await userManager.FindByEmailAsync("albus.dumbledore@acmeglobal.ie");
            var snapeUser = await userManager.FindByEmailAsync("severus.snape@acmeglobal.ie");
            var mcgonagallUser = await userManager.FindByEmailAsync("minerva.mcgonagall@acmeglobal.ie");

            var hermioneUser = await userManager.FindByEmailAsync("hermione.granger@acmeglobal.ie");
            var harryUser = await userManager.FindByEmailAsync("harry.potter@acmeglobal.ie");
            var lunaUser = await userManager.FindByEmailAsync("luna.lovegood@acmeglobal.ie");

            if (dumbledoreUser == null || snapeUser == null || mcgonagallUser == null ||
                hermioneUser == null || harryUser == null || lunaUser == null)
            {
                throw new InvalidOperationException("One or more seeded Identity users were not found after user creation.");
            }

            // FACULTY PROFILES
            if (!await context.FacultyProfiles.AnyAsync())
            {
                context.FacultyProfiles.AddRange(
                    new FacultyProfile
                    {
                        IdentityUserId = dumbledoreUser.Id,
                        FirstName = "Albus",
                        LastName = "Dumbledore",
                        Email = dumbledoreUser.Email!,
                        Phone = "0851000001"
                    },
                    new FacultyProfile
                    {
                        IdentityUserId = snapeUser.Id,
                        FirstName = "Severus",
                        LastName = "Snape",
                        Email = snapeUser.Email!,
                        Phone = "0851000002"
                    },
                    new FacultyProfile
                    {
                        IdentityUserId = mcgonagallUser.Id,
                        FirstName = "Minerva",
                        LastName = "McGonagall",
                        Email = mcgonagallUser.Email!,
                        Phone = "0851000003"
                    }
                );

                await context.SaveChangesAsync();
            }

            // STUDENT PROFILES
            if (!await context.StudentProfiles.AnyAsync())
            {
                context.StudentProfiles.AddRange(
                    new StudentProfile
                    {
                        IdentityUserId = hermioneUser.Id,
                        StudentNumber = "AGC2026001",
                        FirstName = "Hermione",
                        LastName = "Granger",
                        Email = hermioneUser.Email!,
                        Phone = "0852000001",
                        Address = "Phibsborough, Dublin 7",
                        DateOfBirth = new DateTime(2004, 9, 19)
                    },
                    new StudentProfile
                    {
                        IdentityUserId = harryUser.Id,
                        StudentNumber = "AGC2026002",
                        FirstName = "Harry",
                        LastName = "Potter",
                        Email = harryUser.Email!,
                        Phone = "0852000002",
                        Address = "Drumcondra, Dublin 9",
                        DateOfBirth = new DateTime(2004, 7, 31)
                    },
                    new StudentProfile
                    {
                        IdentityUserId = lunaUser.Id,
                        StudentNumber = "AGC2026003",
                        FirstName = "Luna",
                        LastName = "Lovegood",
                        Email = lunaUser.Email!,
                        Phone = "0852000003",
                        Address = "Ranelagh, Dublin 6",
                        DateOfBirth = new DateTime(2004, 2, 13)
                    }
                );

                await context.SaveChangesAsync();
            }

            var dumbledoreProfile = await context.FacultyProfiles.FirstAsync(f => f.Email == "albus.dumbledore@acmeglobal.ie");
            var snapeProfile = await context.FacultyProfiles.FirstAsync(f => f.Email == "severus.snape@acmeglobal.ie");
            var mcgonagallProfile = await context.FacultyProfiles.FirstAsync(f => f.Email == "minerva.mcgonagall@acmeglobal.ie");

            var hermioneProfile = await context.StudentProfiles.FirstAsync(s => s.Email == "hermione.granger@acmeglobal.ie");
            var harryProfile = await context.StudentProfiles.FirstAsync(s => s.Email == "harry.potter@acmeglobal.ie");
            var lunaProfile = await context.StudentProfiles.FirstAsync(s => s.Email == "luna.lovegood@acmeglobal.ie");

            // FACULTY ASSIGNMENTS
            if (!await context.FacultyCourseAssignments.AnyAsync())
            {
                context.FacultyCourseAssignments.AddRange(
                    new FacultyCourseAssignment
                    {
                        FacultyProfileId = dumbledoreProfile.Id,
                        CourseId = biologicalSciences.Id,
                        IsTutor = true
                    },
                    new FacultyCourseAssignment
                    {
                        FacultyProfileId = snapeProfile.Id,
                        CourseId = pharmaceuticalScience.Id,
                        IsTutor = true
                    },
                    new FacultyCourseAssignment
                    {
                        FacultyProfileId = mcgonagallProfile.Id,
                        CourseId = computerScience.Id,
                        IsTutor = true
                    }
                );

                await context.SaveChangesAsync();
            }

            // ENROLMENTS
            if (!await context.CourseEnrolments.AnyAsync())
            {
                context.CourseEnrolments.AddRange(
                    new CourseEnrolment
                    {
                        StudentProfileId = hermioneProfile.Id,
                        CourseId = computerScience.Id,
                        EnrolDate = new DateTime(2026, 9, 5),
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = harryProfile.Id,
                        CourseId = computerScience.Id,
                        EnrolDate = new DateTime(2026, 9, 5),
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = lunaProfile.Id,
                        CourseId = biologicalSciences.Id,
                        EnrolDate = new DateTime(2026, 9, 6),
                        Status = "Active"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}