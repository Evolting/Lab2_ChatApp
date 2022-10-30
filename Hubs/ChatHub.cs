using Lab2_ChatApp.Entity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lab2_ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private ChatAppDBContext context = new ChatAppDBContext();

        public async Task SendMess(string username, string chatId, string message)
        {
            int cId = Int32.Parse(chatId);

            User u = context.Users.FirstOrDefault(u => u.Username == username);

            Message m = new Message();
            m.Content = message;
            m.Timestamp = DateTime.Now;
            m.ChatId = Int32.Parse(chatId);
            m.UserId = u.UserId;

            context.Messages.Add(m);
            context.SaveChanges();

            //await Clients.All.SendAsync("ReceivedMess", username, message);
            //Chat chat = context.Chats.Include(c => c.Users).FirstOrDefault(c => c.ChatId == cId);
            //List<User> users = chat.Users.Where(u => u.Username != username).ToList();

            //foreach (var user in users)
            //{
            //    User u = UserList.GetUser(user.Username);
            //    if (u == null) return;
            //    await Clients.Client(u.ConnectionId).SendAsync("ReceivedMess", username, message);
            //}

            await Clients.Group(chatId).SendAsync("ReceivedMess", username, message);

            //await Clients.Caller.SendAsync("ReceivedMess", username, message);
        }

        public void SetUserName(String username, string chatId)
        {
            User user = context.Users.FirstOrDefault(u => u.Username == username);
            Console.WriteLine(user != null);
            if(user != null)
            {
                user.ConnectionId = Context.ConnectionId;
                UserList.AddUser(user);

                JoinRoom(chatId);
            }
        }

        public Task JoinRoom(string chatId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
