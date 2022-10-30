using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2_ChatApp.Entity
{
    public partial class User
    {
        public User()
        {
            Messages = new HashSet<Message>();
            Chats = new HashSet<Chat>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }

        [NotMapped]
        public String ConnectionId { get; set; }
    }

    public class UserList
    {
        public static List<User> Users = new List<User>();

        public static void AddUser(User user)
        {
            Users.Add(user);
        }

        public static User GetUser(string userName)
        {
            return Users.FirstOrDefault(x => x.Username.Equals(userName));
        }
    }
}
