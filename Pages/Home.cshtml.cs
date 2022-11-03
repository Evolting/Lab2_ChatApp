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

        public void OnGet()
        {

        }

        public void OnPostLogin(String username, String password)
        {
            User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if(user != null)
            {
                List<Chat> chats = context.Chats.Include(c => c.Users).ToList();

                List<Chat> relatedChat = new List<Chat>();

                for (int i = 0; i < chats.Count; i++)
                {
                    if (checkContains(chats[i].Users.ToList(), username))
                    {
                        relatedChat.Add(chats[i]);
                    }
                }
                List<User> users = context.Users.Where(u => u.Username != username).ToList();

                ViewData["users"] = users;
                ViewData["chats"] = relatedChat;
                ViewData["username"] = username;
            }
        }

        public void OnPostPrivate(String username, String chatName, String user)
        {
            Chat chat = new Chat();
            chat.ChatName = chatName;
            chat.ChatType = "Private";

            User u1 = context.Users.FirstOrDefault(u => u.Username == username);
            User u2 = context.Users.FirstOrDefault(u => u.Username == user);

            chat.Users.Add(u1);
            chat.Users.Add(u2);

            context.Chats.Add(chat);
            context.SaveChanges();

            List<Chat> chats = context.Chats.Include(c => c.Users).ToList();

            List<Chat> relatedChat = new List<Chat>();

            for (int i = 0; i < chats.Count; i++)
            {
                if (checkContains(chats[i].Users.ToList(), username))
                {
                    relatedChat.Add(chats[i]);
                }
            }
            List<User> users = context.Users.Where(u => u.Username != username).ToList();

            ViewData["users"] = users;
            ViewData["chats"] = relatedChat;
            ViewData["username"] = username;
        }

        public void OnPostGroup(String username, String chatName, List<int> users)
        {

            Chat chat = new Chat(); 
            chat.ChatName = chatName;
            chat.ChatType = "Group";
            
            foreach(var uId in users)
            {
                User us = context.Users.FirstOrDefault(u => u.UserId == uId);

                chat.Users.Add(us);
            }

            User u1 = context.Users.FirstOrDefault(u => u.Username == username);
            chat.Users.Add(u1);

            context.Chats.Add(chat);
            context.SaveChanges();

            List<Chat> chats = context.Chats.Include(c => c.Users).ToList();

            List<Chat> relatedChat = new List<Chat>();

            for (int i = 0; i < chats.Count; i++)
            {
                if (checkContains(chats[i].Users.ToList(), username))
                {
                    relatedChat.Add(chats[i]);
                }
            }
            List<User> allUsers = context.Users.Where(u => u.Username != username).ToList();

            ViewData["users"] = allUsers;
            ViewData["chats"] = relatedChat;
            ViewData["username"] = username;
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
