using System;
using System.Collections.Generic;

namespace Lab2_ChatApp.Entity
{
    public partial class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
            Users = new HashSet<User>();
        }

        public int ChatId { get; set; }
        public string ChatName { get; set; } = null!;
        public string ChatType { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
