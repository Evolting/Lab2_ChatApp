using Lab2_ChatApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Lab2_ChatApp.Pages
{
    public class HomeModel : PageModel
    {
        private ChatAppDBContext context = new ChatAppDBContext();

        public void OnGet(String username)
        {
            int u = UserList.Users.Count;
            ViewData["username"] = username;

            Chat chat = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == 1);
            List<User> users1 = chat.Users.Where(u => u.Username != username).ToList();

            List<Chat> chats = context.Chats.Include(c => c.Users) .ToList();

            List<Chat> relatedChat = new List<Chat>();

            for(int i = 0; i < chats.Count; i++)
            {
                if (checkContains(chats[i].Users.ToList(), username))
                {
                    relatedChat.Add(chats[i]);
                }
            }
            List<User> users = context.Users.Where(u => u.Username != username).ToList();

            ViewData["users"] = users;
            ViewData["chats"] = relatedChat;
        }

        public void OnPostPrivate(String username, String chatName, String user)
        {
            List<Chat> chats = context.Chats.Include(c => c.Users).Where(c => c.ChatType.Equals("Private")).ToList();
        }

        public IActionResult OnPostGroup(String username, String chatName, List<int> users)
        {
            List<Chat> chats = context.Chats.Include(c => c.Users).Where(c => c.ChatType.Equals("Group")).ToList();

            Chat chat = new Chat(); 
            chat.ChatName = chatName;
            chat.ChatType = "Group";
            
            foreach(var uId in users)
            {
                User us = context.Users.FirstOrDefault(u => u.UserId == uId);

                chat.Users.Add(us);
            }

            chat.Users.Add(UserList.GetUser(username));

            context.Chats.Add(chat);
            context.SaveChanges();

            return Redirect("/Home?usesname="+username);
        }

        public bool checkContains(List<User> users, String username)
        {
            for(int i=0; i < users.Count; i++)
            {
                if (users[i].Username == username) return true;
            }
            return false;
        }
    }
}
