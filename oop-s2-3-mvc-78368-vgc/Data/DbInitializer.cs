using Microsoft.AspNetCore.Identity;
using oop_s2_3_mvc_78368_vgc.Models;

namespace oop_s2_3_mvc_78368_vgc.Data
{
    public static class DbInitializer
    {
        // -----------------------------
        // 1. ROLES
        // -----------------------------
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
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

        // -----------------------------
        // 2. USERS + PROFILES + DATA
        // -----------------------------
        public static async Task SeedData(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // =========================
            // ADMIN
            // =========================
            var admin = await userManager.FindByEmailAsync("admin@test.com");

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Password123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // =========================
            // FACULTY USER
            // =========================
            var facultyUser = await userManager.FindByEmailAsync("faculty@test.com");

            if (facultyUser == null)
            {
                facultyUser = new IdentityUser
                {
                    UserName = "faculty@test.com",
                    Email = "faculty@test.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(facultyUser, "Password123!");
                await userManager.AddToRoleAsync(facultyUser, "Faculty");
            }

            // =========================
            // STUDENT USER
            // =========================
            var studentUser = await userManager.FindByEmailAsync("student@test.com");

            if (studentUser == null)
            {
                studentUser = new IdentityUser
                {
                    UserName = "student@test.com",
                    Email = "student@test.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(studentUser, "Password123!");
                await userManager.AddToRoleAsync(studentUser, "Student");
            }

            // =========================
            // BRANCH
            // =========================
            if (!context.Branches.Any())
            {
                context.Branches.Add(new Branch
                {
                    Name = "Main Campus",
                    Address = "Dublin City Campus"
                });

                context.SaveChanges();
            }

            var branch = context.Branches.First();

            // =========================
            // FACULTY PROFILE (MUST COME BEFORE COURSE)
            // =========================
            if (!context.FacultyProfiles.Any(f => f.IdentityUserId == facultyUser.Id))
            {
                context.FacultyProfiles.Add(new FacultyProfile
                {
                    IdentityUserId = facultyUser.Id,
                    Name = "Demo Faculty",
                    Email = facultyUser.Email
                });

                context.SaveChanges();
            }

            var faculty = context.FacultyProfiles.First(f => f.IdentityUserId == facultyUser.Id);

            // =========================
            // COURSE (NOW HAS FacultyId FIX)
            // =========================
            if (!context.Courses.Any())
            {
                context.Courses.Add(new Course
                {
                    Name = "Computer Science",
                    BranchId = branch.Id,
                    FacultyId = faculty.Id, // 🔥 CRITICAL FIX
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(6)
                });

                context.SaveChanges();
            }

            var course = context.Courses.First();

            // =========================
            // STUDENT PROFILE
            // =========================
            if (!context.StudentProfiles.Any(s => s.IdentityUserId == studentUser.Id))
            {
                context.StudentProfiles.Add(new StudentProfile
                {
                    IdentityUserId = studentUser.Id,
                    Name = "Demo Student",
                    Email = studentUser.Email,
                    Phone = "0000000000",
                    Address = "Student Address",
                    StudentNumber = "STU1001"
                });

                context.SaveChanges();
            }

            var studentProfile = context.StudentProfiles.First(s => s.IdentityUserId == studentUser.Id);

            // =========================
            // ENROLMENT
            // =========================
            if (!context.CourseEnrolments.Any())
            {
                context.CourseEnrolments.Add(new CourseEnrolment
                {
                    StudentProfileId = studentProfile.Id,
                    CourseId = course.Id,
                    EnrolDate = DateTime.Now,
                    Status = "Active"
                });

                context.SaveChanges();
            }

            var enrolment = context.CourseEnrolments.First();

            // =========================
            // ASSIGNMENT
            // =========================
            if (!context.Assignments.Any())
            {
                context.Assignments.Add(new Assignment
                {
                    CourseId = course.Id,
                    Title = "Intro Project",
                    MaxScore = 100,
                    DueDate = DateTime.Now.AddDays(30)
                });

                context.SaveChanges();
            }

            var assignment = context.Assignments.First();

            // =========================
            // ASSIGNMENT RESULT
            // =========================
            if (!context.AssignmentResults.Any())
            {
                context.AssignmentResults.Add(new AssignmentResult
                {
                    AssignmentId = assignment.Id,
                    StudentProfileId = studentProfile.Id,
                    Score = 85,
                    Feedback = "Good work"
                });

                context.SaveChanges();
            }

            // =========================
            // EXAM
            // =========================
            if (!context.Exams.Any())
            {
                context.Exams.Add(new Exam
                {
                    CourseId = course.Id,
                    Title = "Midterm Exam",
                    Date = DateTime.Now.AddMonths(1),
                    MaxScore = 100,
                    ResultsReleased = false
                });

                context.SaveChanges();
            }

            var exam = context.Exams.First();

            // =========================
            // EXAM RESULT
            // =========================
            if (!context.ExamResults.Any())
            {
                context.ExamResults.Add(new ExamResult
                {
                    ExamId = exam.Id,
                    StudentProfileId = studentProfile.Id,
                    Score = 78,
                    Grade = "B"
                });

                context.SaveChanges();
            }
        }
    }
}
