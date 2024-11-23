using System;
using System.Collections.Generic;

namespace WebAPIProHelp.Models
{
    public partial class User
    {
        public User()
        {
            ConsumedDishes = new HashSet<ConsumedDish>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public byte[]? ProfileImage { get; set; }
        public int RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<ConsumedDish> ConsumedDishes { get; set; }
    }
}
