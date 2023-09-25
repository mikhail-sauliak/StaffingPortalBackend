using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StaffingPortalBackend.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Применение миграций
            context.Database.Migrate();

            // Проверка наличия данных
            if (context.Projects.Any())
            {
                return; // База данных уже инициализирована
            }

            // Добавление тестовых проектов
            var projects = new Project[]
            {
                new Project { Name = "Project1", TechStack = "C#, SQL", StartDate = "2022-01-01", EndDate = "2022-12-31", Comments = "Test project 1" },
                new Project { Name = "Project2", TechStack = "Java, MySQL", StartDate = "2022-02-01", EndDate = "2022-12-31", Comments = "Test project 2" }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            // Добавление тестовых пользователей
            var people = new Person[]
            {
                new Person { FirstName = "John", LastName = "Doe", Location = "New York", DivisionManager = "Alice", ResourceManager = "Bob", AvailableFrom = "2022-01-15", Comments = "Available soon" },
                new Person { FirstName = "Jane", LastName = "Doe", Location = "San Francisco", DivisionManager = "Charlie", ResourceManager = "David", AvailableFrom = "2022-02-01", Comments = "Available in February" }
            };

            context.People.AddRange(people);
            context.SaveChanges();
        }
    }
}
