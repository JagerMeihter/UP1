using System.Collections.Generic;

namespace UP1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; } = 0.0;
        public string Cover { get; set; } = "📖";
        public string Text { get; set; }
        public string Genre { get; set; }   // для совместимости со старым кодом
        public List<string> Genres { get; set; } = new List<string>();
    }
}