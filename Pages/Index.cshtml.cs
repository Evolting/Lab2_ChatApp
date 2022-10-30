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

        public IActionResult OnPost(String username, String password)
        {
            User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            int u = UserList.Users.Count;

            if (user != null)
            {
                return Redirect("/Home?username="+user.Username);
            }

            return Redirect("/Index");
        }
    }
}