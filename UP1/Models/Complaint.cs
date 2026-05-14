using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP1.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public string Type { get; set; }        // Book, User, Review
        public int TargetId { get; set; }       // ID книги / пользователя / отзыва
        public string Reason { get; set; }
        public string ComplainantLogin { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false;
    }
}
