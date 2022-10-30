using Lab2_ChatApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
