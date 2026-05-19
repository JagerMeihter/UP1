using System.Data.Entity;
using System.Linq;
using UP1.Models;

namespace UP1.Data
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            // Роли
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role { Name = "Administrator" });
                context.Roles.Add(new Role { Name = "Author" });
                context.Roles.Add(new Role { Name = "User" });
                context.SaveChanges();
            }

            // Статусы чтения
            if (!context.ReadingStatuses.Any())
            {
                context.ReadingStatuses.Add(new ReadingStatus { Name = "В планах" });
                context.ReadingStatuses.Add(new ReadingStatus { Name = "Читаю" });
                context.ReadingStatuses.Add(new ReadingStatus { Name = "Прочитано" });
                context.ReadingStatuses.Add(new ReadingStatus { Name = "Заброшено" });
                context.SaveChanges();
            }

            // Тестовые пользователи
            if (!context.Users.Any())
            {
                var adminRole = context.Roles.First(r => r.Name == "Administrator");
                var authorRole = context.Roles.First(r => r.Name == "Author");
                var userRole = context.Roles.First(r => r.Name == "User");

                context.Users.Add(new User
                {
                    Login = "admin",
                    PasswordHash = "admin123",
                    Email = "admin@up1.ru",
                    DisplayName = "Администратор",
                    RoleId = adminRole.Id
                });

                context.Users.Add(new User
                {
                    Login = "author",
                    PasswordHash = "author123",
                    Email = "author@up1.ru",
                    DisplayName = "Иван Автор",
                    RoleId = authorRole.Id
                });

                context.Users.Add(new User
                {
                    Login = "user",
                    PasswordHash = "user123",
                    Email = "user@up1.ru",
                    DisplayName = "Петр Пользователь",
                    RoleId = userRole.Id
                });

                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}