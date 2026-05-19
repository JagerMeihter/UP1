using System;

namespace UP1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public string Role { get; set; } = "User";

        public bool IsFrozen { get; set; } = false;
        public string FreezeReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}