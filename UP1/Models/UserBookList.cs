using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UP1.Models
{
    public class UserBookList
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public string Shelf { get; set; } // "В планах", "Читаю", "Прочитано", "Заброшено"
}
}