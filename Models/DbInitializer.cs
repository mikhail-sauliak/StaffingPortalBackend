using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace StaffingPortalBackend.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
            context.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");

            // Удаление данных из таблицы People
            context.Database.ExecuteSqlRaw("DELETE FROM People");
            context.SaveChanges();
            // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.People', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='People';");

            // Удаление данных из таблицы Users
            context.Database.ExecuteSqlRaw("DELETE FROM Users");
            context.SaveChanges();
            // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='Users';");

            // Удаление данных из таблицы ProjectCandidates
            context.Database.ExecuteSqlRaw("DELETE FROM ProjectCandidates");
            context.SaveChanges();

            // Удаление данных из таблицы Projects
            context.Database.ExecuteSqlRaw("DELETE FROM Projects");
            context.SaveChanges();
            // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Projects', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='Projects';");

            // Удаление данных из таблицы SalaryReviews
            context.Database.ExecuteSqlRaw("DELETE FROM SalaryReviews");
            context.SaveChanges();
            // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.SalaryReviews', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='SalaryReviews';");

            // Включите все внешние ключи для таблицы Users обратно
            // context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"");
            context.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
            
            // Применение миграций
            context.Database.Migrate();

            // Check for existing data
            if (context.Projects.Any())
            {
                return; // Database has been initialized
            }

            // Adding test projects
            var projects = new Project[]
            {
            new Project 
            { 
                Name = "Project1", 
                TechStack = "C#, SQL", 
                StartDate = DateTime.Parse("2023-10-15"), 
                EndDate = DateTime.Parse("2024-09-30"), 
                Comments = "Test project 1",
                Search = "Active",
                Signed = true,
                PositionSigned = DateTime.Parse("2023-10-10"),
                PositionClosed = DateTime.Parse("2024-09-29"),
                Location = "New York",
                Stream = "QA",
                Level = "Junior",
                Priority = "High",
                Attachment = "path_to_attachment1"
            },
            new Project 
            { 
                Name = "Project2", 
                TechStack = "Java, MySQL", 
                StartDate = DateTime.Parse("2023-11-20"), 
                EndDate = DateTime.Parse("2024-11-19"), 
                Comments = "Test project 2",
                Search = "Cold",
                Signed = false,
                PositionSigned = DateTime.Parse("2023-11-15"),
                PositionClosed = DateTime.Parse("2024-11-18"),
                Location = "Los Angeles",
                Stream = "SDET",
                Level = "Middle",
                Priority = "Medium",
                Attachment = "path_to_attachment2"
            },
            new Project 
            { 
                Name = "Project3", 
                TechStack = "Python, Django", 
                StartDate = DateTime.Parse("2024-01-05"), 
                EndDate = DateTime.Parse("2024-12-20"), 
                Comments = "Test project 3",
                Search = "Draft",
                Signed = true,
                PositionSigned = DateTime.Parse("2024-01-01"),
                PositionClosed = DateTime.Parse("2024-12-19"),
                Location = "Chicago",
                Stream = "QA",
                Level = "Senior",
                Priority = "Low",
                Attachment = "path_to_attachment3"
            },
            new Project 
            { 
                Name = "Project4", 
                TechStack = "JavaScript, React", 
                StartDate = DateTime.Parse("2024-02-10"), 
                EndDate = DateTime.Parse("2024-12-10"), 
                Comments = "Test project 4",
                Search = "Active Internal",
                Signed = false,
                PositionSigned = DateTime.Parse("2024-02-05"),
                PositionClosed = DateTime.Parse("2024-12-09"),
                Location = "Houston",
                Stream = "SDET",
                Level = "Lead",
                Priority = "High",
                Attachment = "path_to_attachment4"
            },
            new Project 
            { 
                Name = "Project5", 
                TechStack = "PHP, Laravel", 
                StartDate = DateTime.Parse("2024-03-15"), 
                EndDate = DateTime.Parse("2024-12-15"), 
                Comments = "Test project 5",
                Search = "Active",
                Signed = true,
                PositionSigned = DateTime.Parse("2024-03-10"),
                PositionClosed = DateTime.Parse("2024-12-14"),
                Location = "Phoenix",
                Stream = "QA",
                Level = "Principal",
                Priority = "Medium",
                Attachment = "path_to_attachment5"
            }, 
        };

            context.Projects.AddRange(projects);
            context.SaveChanges();            

            // Adding test people
            var people = new List<Person>
            {
                // Head
                new Person 
                { 
                    FirstName = "Alice", 
                    LastName = "Johnson", 
                    Location = "Houston", 
                    AvailableFrom = DateTime.Parse("2024-04-05"), 
                    TechStack = "Ruby, Rails", 
                    Stream = "SDET",
                    TMAware = "Not", 
                    Level = "Principal", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 1,
                    DivisionManagerId = 1,
                    PlannedAssignment = "Project6"
                },
                // DeputyHead
                new Person 
                { 
                    FirstName = "Bob", 
                    LastName = "Martin", 
                    Location = "Phoenix", 
                    AvailableFrom = DateTime.Parse("2024-05-15"), 
                    TechStack = "Go, Echo", 
                    Stream = "QA",
                    TMAware = "Not", 
                    Level = "Junior", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 1,
                    DivisionManagerId = 2,
                    PlannedAssignment = "Project7"
                },
                // Division Managers
                new Person 
                { 
                    FirstName = "Charlie", 
                    LastName = "Brown", 
                    Location = "Philadelphia", 
                    AvailableFrom = DateTime.Parse("2024-06-10"), 
                    TechStack = "Swift, Vapor", 
                    Stream = "SDET",
                    TMAware = "Not", 
                    Level = "Middle", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 1,
                    DivisionManagerId = 3,
                    PlannedAssignment = "Project8"
                },
                new Person 
                { 
                    FirstName = "Diana", 
                    LastName = "Ross", 
                    Location = "San Antonio", 
                    AvailableFrom = DateTime.Parse("2024-07-20"), 
                    TechStack = "Kotlin, Ktor", 
                    Stream = "QA",
                    TMAware = "Notified", 
                    Level = "Senior", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 1,
                    DivisionManagerId = 4,
                    PlannedAssignment = "Project9"
                },
                // Talent Managers
                new Person 
                { 
                    FirstName = "Eddie", 
                    LastName = "Murphy", 
                    Location = "San Diego", 
                    AvailableFrom = DateTime.Parse("2024-08-05"), 
                    TechStack = "TypeScript, Angular", 
                    Stream = "SDET",
                    TMAware = "Approves", 
                    Level = "Intern", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 2,
                    DivisionManagerId = 2,
                    PlannedAssignment = "Project10"
                },
                new Person 
                { 
                    FirstName = "Frank", 
                    LastName = "Sinatra", 
                    Location = "Las Vegas", 
                    AvailableFrom = DateTime.Parse("2024-09-01"), 
                    TechStack = "Java, Spring", 
                    Stream = "QA",
                    TMAware = "Not", 
                    Level = "Middle", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 3,
                    DivisionManagerId = 3,
                    PlannedAssignment = "Project11"
                },
                // Regular Employees
                new Person 
                { 
                    FirstName = "Grace", 
                    LastName = "Hopper", 
                    Location = "New York", 
                    AvailableFrom = DateTime.Parse("2024-10-01"), 
                    TechStack = "C, COBOL", 
                    Stream = "QA",
                    TMAware = "Notified", 
                    Level = "Senior", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 4,
                    DivisionManagerId = 4,
                    PlannedAssignment = "Project12"
                },
                new Person 
                { 
                    FirstName = "Hannah", 
                    LastName = "Montana", 
                    Location = "Los Angeles", 
                    AvailableFrom = DateTime.Parse("2024-11-01"), 
                    TechStack = "Python, Django", 
                    Stream = "SDET",
                    TMAware = "Approves", 
                    Level = "Junior", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 1,
                    DivisionManagerId = 1,
                    PlannedAssignment = "Project13"
                },
                new Person 
                { 
                    FirstName = "Igor", 
                    LastName = "Stravinsky", 
                    Location = "Paris", 
                    AvailableFrom = DateTime.Parse("2024-12-01"), 
                    TechStack = "Java, Spring Boot", 
                    Stream = "QA",
                    TMAware = "Not", 
                    Level = "Middle", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 2,
                    DivisionManagerId = 2,
                    PlannedAssignment = "Project14"
                },
                new Person 
                { 
                    FirstName = "Jack", 
                    LastName = "Sparrow", 
                    Location = "Caribbean", 
                    AvailableFrom = DateTime.Parse("2025-01-01"), 
                    TechStack = "Go, Gin", 
                    Stream = "SDET",
                    TMAware = "Notified", 
                    Level = "Senior", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 3,
                    DivisionManagerId = 3,
                    PlannedAssignment = "Project15"
                },
                new Person 
                { 
                    FirstName = "Katie", 
                    LastName = "Holmes", 
                    Location = "Cleveland", 
                    AvailableFrom = DateTime.Parse("2025-02-01"), 
                    TechStack = "JavaScript, React", 
                    Stream = "QA",
                    TMAware = "Approves", 
                    Level = "Intern", 
                    AssignmentExistsInGCP = true,
                    TalentManagerId = 4,
                    DivisionManagerId = 4,
                    PlannedAssignment = "Project16"
                },
                new Person 
                { 
                    FirstName = "Liam", 
                    LastName = "Neeson", 
                    Location = "Dublin", 
                    AvailableFrom = DateTime.Parse("2025-03-01"), 
                    TechStack = "TypeScript, Vue.js", 
                    Stream = "SDET",
                    TMAware = "Not", 
                    Level = "Middle", 
                    AssignmentExistsInGCP = false,
                    TalentManagerId = 1,
                    DivisionManagerId = 1, 
                    PlannedAssignment = "Project17"
                }
            };

            context.People.AddRange(people);
            context.SaveChanges();
            
            people[0].DivisionManagerId = people[0].Id; // Alice is her own Division Manager
            people[0].TalentManagerId = people[0].Id;   // Alice is her own Talent Manager

            people[1].DivisionManagerId = people[1].Id; // Bob is his own Division Manager
            people[1].TalentManagerId = people[0].Id;   // Alice is Bob's Talent Manager

            for (int i = 2; i <= 3; i++) // For Charlie and Diana
            {
                people[i].DivisionManagerId = people[i].Id; // They are their own Division Managers
                people[i].TalentManagerId = people[0].Id;   // Alice is their Talent Manager
            }

            for (int i = 4; i <= 5; i++) // For Eddie and Frank
            {
                people[i].DivisionManagerId = i + 1;       // Assigning the next Division Manager
                people[i].TalentManagerId = people[1].Id;   // Bob is their Talent Manager
            }

            context.SaveChanges();

             // Adding relationships between projects and people
            var projectCandidates = new ProjectCandidate[]
            {
                new() { ProjectId = 1, PersonId = 3 },
                new() { ProjectId = 1, PersonId = 4 },
                new() { ProjectId = 2, PersonId = 5 },
                new() { ProjectId = 2, PersonId = 12 },
                new() { ProjectId = 3, PersonId = 6 },
                new() { ProjectId = 3, PersonId = 7 },
                new() { ProjectId = 4, PersonId = 8 },
                new() { ProjectId = 4, PersonId = 9 },
                new() { ProjectId = 5, PersonId = 11 },
                new() { ProjectId = 5, PersonId = 10 }
            };

            var existingProjectCandidates = context.ProjectCandidates
                .Select(pc => new { pc.ProjectId, pc.PersonId })
                .ToList();

            var newProjectCandidates = projectCandidates
                .Where(pc => !existingProjectCandidates.Any(epc => epc.ProjectId == pc.ProjectId && epc.PersonId == pc.PersonId))
                .ToArray();

            context.ProjectCandidates.AddRange(newProjectCandidates);
            context.SaveChanges();
 
            var users = new List<User>
            {
                CreateUser("AliceJohnson", "Test1234", "Head", "SDET", people[0].Id),
                CreateUser("BobMartin", "Test1234", "DeputyHead", "QA", people[1].Id),
                CreateUser("CharlieBrown", "Test1234", "DivisionManager", "SDET", people[2].Id),
                CreateUser("DianaRoss", "Test1234", "DivisionManager", "QA", people[3].Id),
                CreateUser("EddieMurphy", "Test1234", "TalentManager", "SDET", people[4].Id),
                CreateUser("FrankSinatra", "Test1234", "TalentManager", "QA", people[5].Id),
                // ... (and so on for other users)
            };
            

            context.Users.AddRange(users);
            context.SaveChanges();

            foreach (var person in people)
            {
                var username = person.FirstName + person.LastName;
                var user = users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    person.UserId = user.Id;
                }
            }
            context.SaveChanges();
            // Random generator for SalaryReviews
            Random rand = new Random();

            // Adding test SalaryReviews
            var salaryReviews = new List<SalaryReview>();
            foreach (var person in people)
            {
                decimal oldSalary = rand.Next(40000, 70000); // Random salary between 40,000 and 70,000
                decimal proposedSalary = oldSalary + (oldSalary * (decimal)(rand.Next(1, 10) * 0.01)); // Random increase between 1% and 10%

                salaryReviews.Add(new SalaryReview
                {
                    PersonId = person.Id,
                    ReviewDate = DateTime.Now,
                    Justification = "Annual review",
                    OldSalary = oldSalary,
                    ProposedSalary = proposedSalary,
                    IsApproved = true,
                    Currency = "USD",
                    ContractType = "EC",
                    NewSalaryStartDate = DateTime.Now.AddMonths(1),
                    SalaryReceivedDate = DateTime.Now.AddMonths(2),
                    DivisionManagerComments = "Good performance",
                    TalentManagerComments = "Keep up the good work",
                    ReviewResult = "Approved"
                });
            }   
            context.SalaryReviews.AddRange(salaryReviews);
            context.SaveChanges();         
        }
        
        private static User CreateUser(string username, string password, string role, string stream, int personId)
        {
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            return new User 
            {
                Username = username,
                Role = role,
                Stream = stream,
                PasswordHash = Convert.FromBase64String(hashedPassword),
                PasswordSalt = salt,
                PersonId = personId
            };
        }

        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000; 
        public static string HashPassword(string password, byte[] salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(algorithm.GetBytes(KeySize));
        }
        public static byte[] GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }
    }
}  