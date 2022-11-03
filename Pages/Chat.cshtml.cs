using Lab2_ChatApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lab2_ChatApp.Pages
{
    public class ChatModel : PageModel
    {
        private ChatAppDBContext context = new ChatAppDBContext();

        public void OnGet(int chatId, string username)
        {
            Chat c = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == chatId);

            bool isIncluded = false;

            foreach(var us in c.Users)
            {
                if(us.Username == username)
                {
                    isIncluded = true;
                    break;
                }
            }

            int u = UserList.Users.Count;

            List<Message> messages = context.Messages.Include(m => m.User).Where(m => m.ChatId == chatId).ToList();

            if(messages == null)
            {
                messages = new List<Message>();
            }

            ViewData["isIncluded"] = isIncluded;
            ViewData["messages"] = messages;
            ViewData["username"] = username;
            ViewData["chatId"] = chatId;

            List<User> users = context.Users.ToList();
            users = users.Except(c.Users).ToList();

            ViewData["isPrivate"] = c.ChatType.Equals("Private");
            ViewData["users"] = users;
            ViewData["userInChat"] = c.Users.ToList();
        }

        public void OnPostAdd(String username, String chatId, List<int> users)
        {
            Chat chat = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == Int32.Parse(chatId));

            foreach (var uId in users)
            {
                User us = context.Users.FirstOrDefault(u => u.UserId == uId);

                chat.Users.Add(us);
            }

            context.Chats.Update(chat);
            context.SaveChanges();

            context = new ChatAppDBContext();

            chat = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == Int32.Parse(chatId));

            bool isIncluded = false;

            foreach (var us in chat.Users)
            {
                if (us.Username == username)
                {
                    isIncluded = true;
                    break;
                }
            }

            int u = UserList.Users.Count;

            List<Message> messages = context.Messages.Include(m => m.User).Where(m => m.ChatId == Int32.Parse(chatId)).ToList();

            if (messages == null)
            {
                messages = new List<Message>();
            }

            ViewData["isIncluded"] = isIncluded;
            ViewData["messages"] = messages;
            ViewData["username"] = username;
            ViewData["chatId"] = chatId;

            List<User> notInHereUsers = context.Users.ToList();
            notInHereUsers = notInHereUsers.Except(chat.Users).ToList();

            ViewData["isPrivate"] = chat.ChatType.Equals("Private");
            ViewData["users"] = notInHereUsers;
            ViewData["userInChat"] = chat.Users.ToList();
        }

        public IActionResult OnPostLeave(String username, String chatId)
        {
            Chat chat = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == Int32.Parse(chatId));

            User u = context.Users.FirstOrDefault(u => u.Username.Equals(username));
            chat.Users.Remove(u);
            context.SaveChanges();

            return Redirect("/Home");
        }
    }
}
