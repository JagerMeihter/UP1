using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            Books = File.Exists(BooksFile)
                ? JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(BooksFile)) ?? GetSampleBooks()
                : GetSampleBooks();

            Users = File.Exists(UsersFile)
                ? JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(UsersFile)) ?? GetSampleUsers()
                : GetSampleUsers();
        }

        private List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Title = "Тени прошлого", Author = "Анна Смирнова", Rating = 4.8, Cover = "📖", Text = "Текст книги 1...", Genres = new List<string> { "Фантастика" } },
                new Book { Id = 2, Title = "Последний рассвет", Author = "Дмитрий Волков", Rating = 4.5, Cover = "🔍", Text = "Текст книги 2...", Genres = new List<string> { "Детектив" } },
                new Book { Id = 3, Title = "Сердце в огне", Author = "Мария Лебедева", Rating = 4.9, Cover = "❤️", Text = "Текст книги 3...", Genres = new List<string> { "Романтика" } }
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

        public void SaveBooks() => File.WriteAllText(BooksFile, JsonConvert.SerializeObject(Books, Formatting.Indented));
        public void SaveUsers() => File.WriteAllText(UsersFile, JsonConvert.SerializeObject(Users, Formatting.Indented));
        public List<Book> GetBooksOnShelf(int userId, string shelf)
        {
            // Пока возвращаем все книги (для совместимости)
            return Books.ToList();
        }

        public void AddBookToShelf(int userId, int bookId, string shelf)
        {
            // Пока просто сообщение
            // В будущем здесь будет логика сохранения
        }

        public void SaveReviews() { }   // заглушка
    }
}