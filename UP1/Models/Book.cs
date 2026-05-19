using System;
using System.Collections.Generic;

namespace UP1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
        public string Content { get; set; }           // текст книги

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public bool IsFrozen { get; set; } = false;
        public string FreezeReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}