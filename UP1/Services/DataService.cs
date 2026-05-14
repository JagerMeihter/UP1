using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UP1.Models;

namespace UP1.Services
{
    public class DataService
    {
        private static readonly string BooksFile = "Data/books.json";
        private static readonly string UsersFile = "Data/users.json";

        public List<Book> Books { get; private set; }
        public List<User> Users { get; private set; }

        public DataService()
        {
            LoadData();
        }

        private void LoadData()
        {
            // Создаём папку Data если её нет
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            // Загрузка книг
            if (File.Exists(BooksFile))
            {
                var json = File.ReadAllText(BooksFile);
                Books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
            }
            else
            {
                Books = GetSampleBooks();
                SaveBooks();
            }

            // Загрузка пользователей
            if (File.Exists(UsersFile))
            {
                var json = File.ReadAllText(UsersFile);
                Users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            else
            {
                Users = GetSampleUsers();
                SaveUsers();
            }
        }

        public void SaveBooks() => File.WriteAllText(BooksFile, JsonConvert.SerializeObject(Books, Formatting.Indented));
        public void SaveUsers() => File.WriteAllText(UsersFile, JsonConvert.SerializeObject(Users, Formatting.Indented));

        private List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Title = "Тени прошлого", Author = "Анна Смирнова", Rating = 4.8, Genre = "Фантастика", Cover = "📖", Description = "Увлекательная история о...", Text = "Глава 1...\nТекст книги здесь..." },
                new Book { Id = 2, Title = "Последний рассвет", Author = "Дмитрий Волков", Rating = 4.5, Genre = "Детектив", Cover = "🔍", Description = "Детективный триллер...", Text = "Текст книги..." },
                new Book { Id = 3, Title = "Сердце в огне", Author = "Мария Лебедева", Rating = 4.9, Genre = "Романтика", Cover = "❤️", Description = "Романтическая история...", Text = "Текст книги..." }
            };
        }

        private List<User> GetSampleUsers()
        {
            return new List<User>
            {
                new User { Id = 1, Login = "admin", Password = "admin123", FullName = "Администратор", Email = "admin@up1.ru", Role = "Administrator" },
                new User { Id = 2, Login = "author", Password = "author123", FullName = "Иван Автор", Email = "author@up1.ru", Role = "Author" },
                new User { Id = 3, Login = "user", Password = "user123", FullName = "Петр Пользователь", Email = "user@up1.ru", Role = "User" }
            };
        }
    }
}