using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public string Cover { get; set; }           // эмодзи или путь к картинке
        public string Text { get; set; }            // текст книги для чтения
        public int AuthorId { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
    }
}