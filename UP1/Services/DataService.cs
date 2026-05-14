using Newtonsoft.Json;
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
        private static readonly string UserListsFile = "Data/userlists.json";

        public List<Book> Books { get; private set; }
        public List<User> Users { get; private set; }
        public List<UserBookList> UserLists { get; private set; } = new List<UserBookList>();

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

            // Загрузка списков пользователей (полки книг)
            if (File.Exists(UserListsFile))
            {
                var json = File.ReadAllText(UserListsFile);
                UserLists = JsonConvert.DeserializeObject<List<UserBookList>>(json) ?? new List<UserBookList>();
            }
            else
            {
                UserLists = new List<UserBookList>();
                SaveUserLists();
            }
        }

        public void SaveBooks() => File.WriteAllText(BooksFile, JsonConvert.SerializeObject(Books, Formatting.Indented));
        public void SaveUsers() => File.WriteAllText(UsersFile, JsonConvert.SerializeObject(Users, Formatting.Indented));
        public void SaveUserLists() => File.WriteAllText(UserListsFile, JsonConvert.SerializeObject(UserLists, Formatting.Indented));

        // Получить книги пользователя на определённой полке
        public List<Book> GetBooksOnShelf(int userId, string shelf)
        {
            var bookIds = UserLists
                .Where(ul => ul.UserId == userId && ul.Shelf == shelf)
                .Select(ul => ul.BookId)
                .ToList();

            return Books.Where(b => bookIds.Contains(b.Id)).ToList();
        }

        // Добавить/переместить книгу на полку
        public void AddBookToShelf(int userId, int bookId, string shelf)
        {
            // Удаляем книгу со всех полок этого пользователя
            UserLists.RemoveAll(ul => ul.UserId == userId && ul.BookId == bookId);

            // Добавляем на новую полку
            UserLists.Add(new UserBookList
            {
                UserId = userId,
                BookId = bookId,
                Shelf = shelf
            });

            SaveUserLists();
        }

        private List<Book> GetSampleBooks()
        {
            return new List<Book>
    {
        new Book
        {
            Id = 1,
            Title = "Тени прошлого",
            Author = "Анна Смирнова",
            Rating = 4.8,
            Cover = "📖",
            Description = "Увлекательная история о тайнах...",
            Text = "Глава 1...",
            Genres = new List<string> { "Фантастика", "Мистика" }
        },
        new Book
        {
            Id = 2,
            Title = "Последний рассвет",
            Author = "Дмитрий Волков",
            Rating = 4.5,
            Cover = "🔍",
            Description = "Детективный триллер...",
            Text = "Текст...",
            Genres = new List<string> { "Детектив", "Триллер" }
        },
        new Book
        {
            Id = 3,
            Title = "Сердце в огне",
            Author = "Мария Лебедева",
            Rating = 4.9,
            Cover = "❤️",
            Description = "Романтическая история...",
            Text = "Текст...",
            Genres = new List<string> { "Романтика", "Драма" }
        }
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