using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UP1.Data;
using UP1.Models;

namespace UP1.Services
{
    public class DataService
    {
        private readonly AppDbContext db = new AppDbContext();

        public User GetUser(string login, string password)
        {
            return db.Users.Include(u => u.Role)
                           .FirstOrDefault(u => u.Login == login && u.PasswordHash == password);
        }

        public List<Book> GetAllBooks()
        {
            return db.Books.Include(b => b.Author).ToList();
        }

        public List<Review> GetReviewsForBook(int bookId)
        {
            return db.Reviews.Include(r => r.User)
                             .Where(r => r.BookId == bookId)
                             .OrderByDescending(r => r.CreatedAt)
                             .ToList();
        }

        public void AddReview(Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }

        public List<Book> GetBooksOnShelf(int userId, string statusName)
        {
            var status = db.ReadingStatuses.FirstOrDefault(s => s.Name == statusName);
            if (status == null) return new List<Book>();

            return db.UserBookLists
                     .Where(ubl => ubl.UserId == userId && ubl.StatusId == status.Id)
                     .Select(ubl => ubl.Book)
                     .Include(b => b.Author)
                     .ToList();
        }

        public void AddBookToShelf(int userId, int bookId, string statusName)
        {
            var status = db.ReadingStatuses.FirstOrDefault(s => s.Name == statusName);
            if (status == null) return;

            var existing = db.UserBookLists.Where(ubl => ubl.UserId == userId && ubl.BookId == bookId);
            db.UserBookLists.RemoveRange(existing);

            db.UserBookLists.Add(new UserBookList
            {
                UserId = userId,
                BookId = bookId,
                StatusId = status.Id
            });

            db.SaveChanges();
        }

        public void SaveChanges() => db.SaveChanges();
        public void UpdateBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddBook(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
        }
    }
}