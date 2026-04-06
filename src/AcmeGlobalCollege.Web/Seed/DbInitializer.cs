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
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(
                userManager,
                "admin@acmeglobal.ie",
                "AcmeVGC!26",
                "Admin");

            await CreateUserIfNotExistsAsync(
                userManager,
                "albus.dumbledore@acmeglobal.ie",
                "Dumbledore!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "severus.snape@acmeglobal.ie",
                "Snape!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "minerva.mcgonagall@acmeglobal.ie",
                "McGonagall!26",
                "Faculty");

            await CreateUserIfNotExistsAsync(
                userManager,
                "hermione.granger@acmeglobal.ie",
                "Hermione!26",
                "Student");

            await CreateUserIfNotExistsAsync(
                userManager,
                "harry.potter@acmeglobal.ie",
                "Harry!26",
                "Student");

            await CreateUserIfNotExistsAsync(
                userManager,
                "luna.lovegood@acmeglobal.ie",
                "Luna!26",
                "Student");
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
                    await userManager.AddToRoleAsync(existingUser, role);
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

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }

        private static async Task SeedAcademicDataAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            if (await context.Branches.AnyAsync())
            {
                return;
            }

            var dublinBranch = new Branch
            {
                Name = "Dublin City Branch",
                Address = "15 College Green, Dublin 2"
            };

            var tallaghtBranch = new Branch
            {
                Name = "Tallaght Branch",
                Address = "Blessington Road, Tallaght, Dublin 24"
            };

            var blanchardstownBranch = new Branch
            {
                Name = "Blanchardstown Branch",
                Address = "Main Street, Blanchardstown, Dublin 15"
            };

            context.Branches.AddRange(dublinBranch, tallaghtBranch, blanchardstownBranch);
            await context.SaveChangesAsync();

            var computerScience = new Course
            {
                Name = "BSc Computer Science",
                BranchId = dublinBranch.Id,
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2030, 5, 31)
            };

            var pharmaceuticalScience = new Course
            {
                Name = "BSc Pharmaceutical and Chemical Science",
                BranchId = tallaghtBranch.Id,
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2030, 5, 31)
            };

            var biologicalSciences = new Course
            {
                Name = "BSc Biological Sciences",
                BranchId = blanchardstownBranch.Id,
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2030, 5, 31)
            };

            var engineering = new Course
            {
                Name = "BSc Mechanical and Manufacturing Engineering",
                BranchId = tallaghtBranch.Id,
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2030, 5, 31)
            };

            context.Courses.AddRange(
                computerScience,
                pharmaceuticalScience,
                biologicalSciences,
                engineering);

            await context.SaveChangesAsync();

            var modules = new List<Module>
            {
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
            };

            context.Modules.AddRange(modules);
            await context.SaveChangesAsync();

            var dumbledoreUser = await userManager.FindByEmailAsync("albus.dumbledore@acmeglobal.ie");
            var snapeUser = await userManager.FindByEmailAsync("severus.snape@acmeglobal.ie");
            var mcgonagallUser = await userManager.FindByEmailAsync("minerva.mcgonagall@acmeglobal.ie");

            var hermioneUser = await userManager.FindByEmailAsync("hermione.granger@acmeglobal.ie");
            var harryUser = await userManager.FindByEmailAsync("harry.potter@acmeglobal.ie");
            var lunaUser = await userManager.FindByEmailAsync("luna.lovegood@acmeglobal.ie");

            if (dumbledoreUser == null || snapeUser == null || mcgonagallUser == null ||
                hermioneUser == null || harryUser == null || lunaUser == null)
            {
                return;
            }

            var dumbledoreProfile = new FacultyProfile
            {
                IdentityUserId = dumbledoreUser.Id,
                FirstName = "Albus",
                LastName = "Dumbledore",
                Email = dumbledoreUser.Email!,
                Phone = "0851000001"
            };

            var snapeProfile = new FacultyProfile
            {
                IdentityUserId = snapeUser.Id,
                FirstName = "Severus",
                LastName = "Snape",
                Email = snapeUser.Email!,
                Phone = "0851000002"
            };

            var mcgonagallProfile = new FacultyProfile
            {
                IdentityUserId = mcgonagallUser.Id,
                FirstName = "Minerva",
                LastName = "McGonagall",
                Email = mcgonagallUser.Email!,
                Phone = "0851000003"
            };

            context.FacultyProfiles.AddRange(dumbledoreProfile, snapeProfile, mcgonagallProfile);
            await context.SaveChangesAsync();

            var hermioneProfile = new StudentProfile
            {
                IdentityUserId = hermioneUser.Id,
                StudentNumber = "AGC2026001",
                FirstName = "Hermione",
                LastName = "Granger",
                Email = hermioneUser.Email!,
                Phone = "0852000001",
                Address = "Phibsborough, Dublin 7",
                DateOfBirth = new DateTime(2004, 9, 19)
            };

            var harryProfile = new StudentProfile
            {
                IdentityUserId = harryUser.Id,
                StudentNumber = "AGC2026002",
                FirstName = "Harry",
                LastName = "Potter",
                Email = harryUser.Email!,
                Phone = "0852000002",
                Address = "Drumcondra, Dublin 9",
                DateOfBirth = new DateTime(2004, 7, 31)
            };

            var lunaProfile = new StudentProfile
            {
                IdentityUserId = lunaUser.Id,
                StudentNumber = "AGC2026003",
                FirstName = "Luna",
                LastName = "Lovegood",
                Email = lunaUser.Email!,
                Phone = "0852000003",
                Address = "Ranelagh, Dublin 6",
                DateOfBirth = new DateTime(2004, 2, 13)
            };

            context.StudentProfiles.AddRange(hermioneProfile, harryProfile, lunaProfile);
            await context.SaveChangesAsync();

            var facultyAssignments = new List<FacultyCourseAssignment>
            {
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
            };

            context.FacultyCourseAssignments.AddRange(facultyAssignments);
            await context.SaveChangesAsync();

            var enrolments = new List<CourseEnrolment>
            {
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
            };

            context.CourseEnrolments.AddRange(enrolments);
            await context.SaveChangesAsync();
        }
    }
}