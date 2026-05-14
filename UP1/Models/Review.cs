using System;

namespace UP1.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }           // от 1 до 5
        public DateTime Date { get; set; } = DateTime.Now;
    }
}