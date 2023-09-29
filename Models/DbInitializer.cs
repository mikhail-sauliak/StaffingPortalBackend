using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace StaffingPortalBackend.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.ProjectCandidates.RemoveRange(context.ProjectCandidates);
            context.People.RemoveRange(context.People);
            context.Projects.RemoveRange(context.Projects);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.People', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Projects', RESEED, 0);");
            // Apply migrations
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
            new Project 
            { 
                Name = "Project6", 
                TechStack = "Ruby, Rails", 
                StartDate = DateTime.Parse("2024-04-01"), 
                EndDate = DateTime.Parse("2024-12-31"), 
                Comments = "Test project 6",
                Search = "Cold",
                Signed = false,
                PositionSigned = DateTime.Parse("2024-03-28"),
                PositionClosed = DateTime.Parse("2024-12-30"),
                Location = "Philadelphia",
                Stream = "SDET",
                Level = "Intern",
                Priority = "Low",
                Attachment = "path_to_attachment6"
            },
            new Project 
            { 
                Name = "Project7", 
                TechStack = "Go, Echo", 
                StartDate = DateTime.Parse("2024-05-10"), 
                EndDate = DateTime.Parse("2024-12-30"), 
                Comments = "Test project 7",
                Search = "Draft",
                Signed = true,
                PositionSigned = DateTime.Parse("2024-05-05"),
                PositionClosed = DateTime.Parse("2024-12-29"),
                Location = "San Antonio",
                Stream = "QA",
                Level = "Junior",
                Priority = "High",
                Attachment = "path_to_attachment7"
            },
            new Project 
            { 
                Name = "Project8", 
                TechStack = "Swift, Vapor", 
                StartDate = DateTime.Parse("2024-06-20"), 
                EndDate = DateTime.Parse("2024-12-29"), 
                Comments = "Test project 8",
                Search = "Active Internal",
                Signed = false,
                PositionSigned = DateTime.Parse("2024-06-15"),
                PositionClosed = DateTime.Parse("2024-12-28"),
                Location = "San Diego",
                Stream = "SDET",
                Level = "Middle",
                Priority = "Medium",
                Attachment = "path_to_attachment8"
            },
            new Project 
            { 
                Name = "Project9", 
                TechStack = "Kotlin, Ktor", 
                StartDate = DateTime.Parse("2024-07-05"), 
                EndDate = DateTime.Parse("2024-12-28"), 
                Comments = "Test project 9",
                Search = "Active",
                Signed = true,
                PositionSigned = DateTime.Parse("2024-07-01"),
                PositionClosed = DateTime.Parse("2024-12-27"),
                Location = "Dallas",
                Stream = "QA",
                Level = "Senior",
                Priority = "Low",
                Attachment = "path_to_attachment9"
            },
            new Project 
            { 
                Name = "Project10", 
                TechStack = "TypeScript, Angular", 
                StartDate = DateTime.Parse("2024-08-15"), 
                EndDate = DateTime.Parse("2024-12-27"), 
                Comments = "Test project 10",
                Search = "Cold",
                Signed = false,
                PositionSigned = DateTime.Parse("2024-08-10"),
                PositionClosed = DateTime.Parse("2024-12-26"),
                Location = "San Jose",
                Stream = "SDET",
                Level = "Lead",
                Priority = "High",
                Attachment = "path_to_attachment10"
            }
        };


            context.Projects.AddRange(projects);
            context.SaveChanges();

            // Adding test people
            var people = new Person[]
            {
                new Person { FirstName = "John", LastName = "Doe", Location = "New York", DivisionManager = "Alice", ResourceManager = "Bob", AvailableFrom = DateTime.Parse("2023-10-15"), Comments = "Available soon", TechStack = "C#, SQL", Stream = "QA", TMAware = "Not", Level = "Junior", AssignmentExistsInGCP = false, PlannedAssignment = "Project1" },
                new Person { FirstName = "Jane", LastName = "Doe", Location = "San Francisco", DivisionManager = "Charlie", ResourceManager = "David", AvailableFrom = DateTime.Parse("2023-11-01"), Comments = "Available in November", TechStack = "Java, MySQL", Stream = "SDET", TMAware = "Notified", Level = "Middle", AssignmentExistsInGCP = true, PlannedAssignment = "Project2" },
                new Person { FirstName = "Mike", LastName = "Jordan", Location = "Chicago", DivisionManager = "Eve", ResourceManager = "Frank", AvailableFrom = DateTime.Parse("2024-01-20"), Comments = "Available in January", TechStack = "Python, Django", Stream = "QA", TMAware = "Approves", Level = "Senior", AssignmentExistsInGCP = false, PlannedAssignment = "Project3" },
                new Person { FirstName = "Sarah", LastName = "Lee", Location = "Los Angeles", DivisionManager = "George", ResourceManager = "Helen", AvailableFrom = DateTime.Parse("2024-02-25"), Comments = "Available in February", TechStack = "JavaScript, React", Stream = "SDET", TMAware = "Not", Level = "Intern", AssignmentExistsInGCP = true, PlannedAssignment = "Project4" },
                new Person { FirstName = "Chris", LastName = "Kim", Location = "Seattle", DivisionManager = "Irene", ResourceManager = "Jack", AvailableFrom = DateTime.Parse("2024-03-10"), Comments = "Available in March", TechStack = "PHP, Laravel", Stream = "QA", TMAware = "Notified", Level = "Lead", AssignmentExistsInGCP = false, PlannedAssignment = "Project5" },
                new Person { FirstName = "Alice", LastName = "Johnson", Location = "Houston", DivisionManager = "Karen", ResourceManager = "Louis", AvailableFrom = DateTime.Parse("2024-04-05"), Comments = "Available in April", TechStack = "Ruby, Rails", Stream = "SDET", TMAware = "Approves", Level = "Principal", AssignmentExistsInGCP = true, PlannedAssignment = "Project6" },
                new Person { FirstName = "Bob", LastName = "Martin", Location = "Phoenix", DivisionManager = "Mandy", ResourceManager = "Nancy", AvailableFrom = DateTime.Parse("2024-05-15"), Comments = "Available in May", TechStack = "Go, Echo", Stream = "QA", TMAware = "Not", Level = "Junior", AssignmentExistsInGCP = false, PlannedAssignment = "Project7" },
                new Person { FirstName = "Charlie", LastName = "Brown", Location = "Philadelphia", DivisionManager = "Olivia", ResourceManager = "Paul", AvailableFrom = DateTime.Parse("2024-06-10"), Comments = "Available in June", TechStack = "Swift, Vapor", Stream = "SDET", TMAware = "Notified", Level = "Middle", AssignmentExistsInGCP = true, PlannedAssignment = "Project8" },
                new Person { FirstName = "Diana", LastName = "Ross", Location = "San Antonio", DivisionManager = "Quincy", ResourceManager = "Rachel", AvailableFrom = DateTime.Parse("2024-07-20"), Comments = "Available in July", TechStack = "Kotlin, Ktor", Stream = "QA", TMAware = "Approves", Level = "Senior", AssignmentExistsInGCP = false, PlannedAssignment = "Project9" },
                new Person { FirstName = "Eddie", LastName = "Murphy", Location = "San Diego", DivisionManager = "Steve", ResourceManager = "Tina", AvailableFrom = DateTime.Parse("2024-08-05"), Comments = "Available in August", TechStack = "TypeScript, Angular", Stream = "SDET", TMAware = "Not", Level = "Intern", AssignmentExistsInGCP = true, PlannedAssignment = "Project10" },
                new Person { FirstName = "Fiona", LastName = "Apple", Location = "Dallas", DivisionManager = "Ursula", ResourceManager = "Victor", AvailableFrom = DateTime.Parse("2024-09-15"), Comments = "Available in September", TechStack = "C#, SQL", Stream = "QA", TMAware = "Notified", Level = "Lead", AssignmentExistsInGCP = false, PlannedAssignment = "Project1" },
                new Person { FirstName = "George", LastName = "Clooney", Location = "San Jose", DivisionManager = "Wendy", ResourceManager = "Xavier", AvailableFrom = DateTime.Parse("2024-10-10"), Comments = "Available in October", TechStack = "Java, MySQL", Stream = "SDET", TMAware = "Approves", Level = "Principal", AssignmentExistsInGCP = true, PlannedAssignment = "Project2" },
                new Person { FirstName = "Helen", LastName = "Mirren", Location = "Austin", DivisionManager = "Yvonne", ResourceManager = "Zach", AvailableFrom = DateTime.Parse("2024-11-20"), Comments = "Available in November", TechStack = "Python, Django", Stream = "QA", TMAware = "Not", Level = "Junior", AssignmentExistsInGCP = false, PlannedAssignment = "Project3" },
                new Person { FirstName = "Irene", LastName = "Cara", Location = "Jacksonville", DivisionManager = "Amy", ResourceManager = "Brian", AvailableFrom = DateTime.Parse("2024-12-05"), Comments = "Available in December", TechStack = "JavaScript, React", Stream = "SDET", TMAware = "Notified", Level = "Middle", AssignmentExistsInGCP = true, PlannedAssignment = "Project4" },
                new Person { FirstName = "Jack", LastName = "Nicholson", Location = "San Francisco", DivisionManager = "Cathy", ResourceManager = "Derek", AvailableFrom = DateTime.Parse("2025-01-15"), Comments = "Available in January", TechStack = "PHP, Laravel", Stream = "QA", TMAware = "Approves", Level = "Senior", AssignmentExistsInGCP = false, PlannedAssignment = "Project5" },
                new Person { FirstName = "Karen", LastName = "O", Location = "Indianapolis", DivisionManager = "Elaine", ResourceManager = "Fred", AvailableFrom = DateTime.Parse("2025-02-10"), Comments = "Available in February", TechStack = "Ruby, Rails", Stream = "SDET", TMAware = "Not", Level = "Intern", AssignmentExistsInGCP = true, PlannedAssignment = "Project6" },
                new Person { FirstName = "Louis", LastName = "Armstrong", Location = "Columbus", DivisionManager = "Gloria", ResourceManager = "Howard", AvailableFrom = DateTime.Parse("2025-03-20"), Comments = "Available in March", TechStack = "Go, Echo", Stream = "QA", TMAware = "Notified", Level = "Lead", AssignmentExistsInGCP = false, PlannedAssignment = "Project7" },
                new Person { FirstName = "Mandy", LastName = "Moore", Location = "Fort Worth", DivisionManager = "Ivy", ResourceManager = "Justin", AvailableFrom = DateTime.Parse("2025-04-05"), Comments = "Available in April", TechStack = "Swift, Vapor", Stream = "SDET", TMAware = "Approves", Level = "Principal", AssignmentExistsInGCP = true, PlannedAssignment = "Project8" },
                new Person { FirstName = "Nancy", LastName = "Sinatra", Location = "Charlotte", DivisionManager = "Kelly", ResourceManager = "Leo", AvailableFrom = DateTime.Parse("2025-05-15"), Comments = "Available in May", TechStack = "Kotlin, Ktor", Stream = "QA", TMAware = "Not", Level = "Junior", AssignmentExistsInGCP = false, PlannedAssignment = "Project9" },
                new Person { FirstName = "Olivia", LastName = "Newton-John", Location = "Detroit", DivisionManager = "Monica", ResourceManager = "Nick", AvailableFrom = DateTime.Parse("2025-06-10"), Comments = "Available in June", TechStack = "TypeScript, Angular", Stream = "SDET", TMAware = "Notified", Level = "Middle", AssignmentExistsInGCP = true, PlannedAssignment = "Project10" }
            };


            context.People.AddRange(people);
            context.SaveChanges();

            // Adding relationships between projects and people
            var projectCandidates = new ProjectCandidate[]
            {
                new() { ProjectId = 1, PersonId = 1 },
                new() { ProjectId = 1, PersonId = 2 },
                new() { ProjectId = 2, PersonId = 3 },
                new() { ProjectId = 2, PersonId = 4 },
                new() { ProjectId = 3, PersonId = 5 },
                new() { ProjectId = 3, PersonId = 6 },
                new() { ProjectId = 4, PersonId = 7 },
                new() { ProjectId = 4, PersonId = 8 },
                new() { ProjectId = 5, PersonId = 9 },
                new() { ProjectId = 5, PersonId = 10 },
                new() { ProjectId = 6, PersonId = 11 },
                new() { ProjectId = 6, PersonId = 12 },
                new() { ProjectId = 7, PersonId = 13 },
                new() { ProjectId = 7, PersonId = 14 },
                new() { ProjectId = 8, PersonId = 15 },
                new() { ProjectId = 8, PersonId = 16 },
                new() { ProjectId = 9, PersonId = 17 },
                new() { ProjectId = 9, PersonId = 18 },
                new() { ProjectId = 10, PersonId = 19 },
                new() { ProjectId = 10, PersonId = 20 }
            };

            context.ProjectCandidates.AddRange(projectCandidates);
            context.SaveChanges();
        }
    }
}
