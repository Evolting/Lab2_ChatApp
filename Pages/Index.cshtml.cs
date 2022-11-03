using Lab2_ChatApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Lab2_ChatApp.Pages
{
    public class IndexModel : PageModel
    {
        private ChatAppDBContext context = new ChatAppDBContext();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public void OnPost(String username, String password)
        {
            User user = new User();
            user.Username = username;
            user.Password = password;

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}