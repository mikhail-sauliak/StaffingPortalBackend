using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StaffingPortalBackend.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
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
                new Project { Name = "Project1", TechStack = "C#, SQL", StartDate = "2022-01-01", EndDate = "2022-12-31", Comments = "Test project 1" },
                new Project { Name = "Project2", TechStack = "Java, MySQL", StartDate = "2022-02-01", EndDate = "2022-12-31", Comments = "Test project 2" },
                new Project { Name = "Project3", TechStack = "Python, Django", StartDate = "2022-03-01", EndDate = "2022-12-31", Comments = "Test project 3" },
                new Project { Name = "Project4", TechStack = "JavaScript, React", StartDate = "2022-04-01", EndDate = "2022-12-31", Comments = "Test project 4" },
                new Project { Name = "Project5", TechStack = "PHP, Laravel", StartDate = "2022-05-01", EndDate = "2022-12-31", Comments = "Test project 5" },
                new Project { Name = "Project6", TechStack = "Ruby, Rails", StartDate = "2022-06-01", EndDate = "2022-12-31", Comments = "Test project 6" },
                new Project { Name = "Project7", TechStack = "Go, Echo", StartDate = "2022-07-01", EndDate = "2022-12-31", Comments = "Test project 7" },
                new Project { Name = "Project8", TechStack = "Swift, Vapor", StartDate = "2022-08-01", EndDate = "2022-12-31", Comments = "Test project 8" },
                new Project { Name = "Project9", TechStack = "Kotlin, Ktor", StartDate = "2022-09-01", EndDate = "2022-12-31", Comments = "Test project 9" },
                new Project { Name = "Project10", TechStack = "TypeScript, Angular", StartDate = "2022-10-01", EndDate = "2022-12-31", Comments = "Test project 10" }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            // Adding test people
            var people = new Person[]
            {
                new Person { FirstName = "John", LastName = "Doe", Location = "New York", DivisionManager = "Alice", ResourceManager = "Bob", AvailableFrom = DateTime.Parse("2022-01-15"), Comments = "Available soon" },
                new Person { FirstName = "Jane", LastName = "Doe", Location = "San Francisco", DivisionManager = "Charlie", ResourceManager = "David", AvailableFrom = DateTime.Parse("2022-02-01"), Comments = "Available in February" },
                new Person { FirstName = "Mike", LastName = "Jordan", Location = "Chicago", DivisionManager = "Eve", ResourceManager = "Frank", AvailableFrom = DateTime.Parse("2022-03-15"), Comments = "Available in March" },
                new Person { FirstName = "Sarah", LastName = "Lee", Location = "Los Angeles", DivisionManager = "George", ResourceManager = "Helen", AvailableFrom = DateTime.Parse("2022-04-15"), Comments = "Available in April" },
                new Person { FirstName = "Chris", LastName = "Kim", Location = "Seattle", DivisionManager = "Irene", ResourceManager = "Jack", AvailableFrom = DateTime.Parse("2022-05-15"), Comments = "Available in May" },
                new Person { FirstName = "Alice", LastName = "Johnson", Location = "Houston", DivisionManager = "Karen", ResourceManager = "Louis", AvailableFrom = DateTime.Parse("2022-06-15"), Comments = "Available in June" },
                new Person { FirstName = "Bob", LastName = "Martin", Location = "Phoenix", DivisionManager = "Mandy", ResourceManager = "Nancy", AvailableFrom = DateTime.Parse("2022-07-15"), Comments = "Available in July" },
                new Person { FirstName = "Charlie", LastName = "Brown", Location = "Philadelphia", DivisionManager = "Olivia", ResourceManager = "Paul", AvailableFrom = DateTime.Parse("2022-08-15"), Comments = "Available in August" },
                new Person { FirstName = "Diana", LastName = "Ross", Location = "San Antonio", DivisionManager = "Quincy", ResourceManager = "Rachel", AvailableFrom = DateTime.Parse("2022-09-15"), Comments = "Available in September" },
                new Person { FirstName = "Eddie", LastName = "Murphy", Location = "San Diego", DivisionManager = "Steve", ResourceManager = "Tina", AvailableFrom = DateTime.Parse("2022-10-15"), Comments = "Available in October" },
                new Person { FirstName = "Fiona", LastName = "Apple", Location = "Dallas", DivisionManager = "Ursula", ResourceManager = "Victor", AvailableFrom = DateTime.Parse("2022-11-15"), Comments = "Available in November" },
                new Person { FirstName = "George", LastName = "Clooney", Location = "San Jose", DivisionManager = "Wendy", ResourceManager = "Xavier", AvailableFrom = DateTime.Parse("2022-12-15"), Comments = "Available in December" },
                new Person { FirstName = "Helen", LastName = "Mirren", Location = "Austin", DivisionManager = "Yvonne", ResourceManager = "Zach", AvailableFrom = DateTime.Parse("2023-01-15"), Comments = "Available in January" },
                new Person { FirstName = "Irene", LastName = "Cara", Location = "Jacksonville", DivisionManager = "Amy", ResourceManager = "Brian", AvailableFrom = DateTime.Parse("2023-02-15"), Comments = "Available in February" },
                new Person { FirstName = "Jack", LastName = "Nicholson", Location = "San Francisco", DivisionManager = "Cathy", ResourceManager = "Derek", AvailableFrom = DateTime.Parse("2023-03-15"), Comments = "Available in March" },
                new Person { FirstName = "Karen", LastName = "O", Location = "Indianapolis", DivisionManager = "Elaine", ResourceManager = "Fred", AvailableFrom = DateTime.Parse("2023-04-15"), Comments = "Available in April" },
                new Person { FirstName = "Louis", LastName = "Armstrong", Location = "Columbus", DivisionManager = "Gloria", ResourceManager = "Howard", AvailableFrom = DateTime.Parse("2023-05-15"), Comments = "Available in May" },
                new Person { FirstName = "Mandy", LastName = "Moore", Location = "Fort Worth", DivisionManager = "Ivy", ResourceManager = "Justin", AvailableFrom = DateTime.Parse("2023-06-15"), Comments = "Available in June" },
                new Person { FirstName = "Nancy", LastName = "Sinatra", Location = "Charlotte", DivisionManager = "Kelly", ResourceManager = "Leo", AvailableFrom = DateTime.Parse("2023-07-15"), Comments = "Available in July" },
                new Person { FirstName = "Olivia", LastName = "Newton-John", Location = "Detroit", DivisionManager = "Monica", ResourceManager = "Nick", AvailableFrom = DateTime.Parse("2023-08-15"), Comments = "Available in August" }
            };

            context.People.AddRange(people);
            context.SaveChanges();

            // Adding relationships between projects and people
            var projectCandidates = new ProjectCandidate[]
            {
                new ProjectCandidate { ProjectId = 1, PersonId = 1 },
                new ProjectCandidate { ProjectId = 1, PersonId = 2 },
                new ProjectCandidate { ProjectId = 2, PersonId = 3 },
                new ProjectCandidate { ProjectId = 2, PersonId = 4 },
                new ProjectCandidate { ProjectId = 3, PersonId = 5 },
                new ProjectCandidate { ProjectId = 3, PersonId = 6 },
                new ProjectCandidate { ProjectId = 4, PersonId = 7 },
                new ProjectCandidate { ProjectId = 4, PersonId = 8 },
                new ProjectCandidate { ProjectId = 5, PersonId = 9 },
                new ProjectCandidate { ProjectId = 5, PersonId = 10 },
                new ProjectCandidate { ProjectId = 6, PersonId = 11 },
                new ProjectCandidate { ProjectId = 6, PersonId = 12 },
                new ProjectCandidate { ProjectId = 7, PersonId = 13 },
                new ProjectCandidate { ProjectId = 7, PersonId = 14 },
                new ProjectCandidate { ProjectId = 8, PersonId = 15 },
                new ProjectCandidate { ProjectId = 8, PersonId = 16 },
                new ProjectCandidate { ProjectId = 9, PersonId = 17 },
                new ProjectCandidate { ProjectId = 9, PersonId = 18 },
                new ProjectCandidate { ProjectId = 10, PersonId = 19 },
                new ProjectCandidate { ProjectId = 10, PersonId = 20 }
            };

            context.ProjectCandidates.AddRange(projectCandidates);
            context.SaveChanges();
        }
    }
}
