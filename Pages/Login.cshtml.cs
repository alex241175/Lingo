using System.Linq;
using System.Threading.Tasks;
using Lingo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;

namespace Lingo.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LingoDbContext context;
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public LoginModel(LingoDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> OnPost(string UserName, string Password)
        {
            User User = context.Users.SingleOrDefault(u => u.UserName == UserName);

            if (User != null ){
                if (User.Password == Password){
                    var user = JsonConvert.SerializeObject(User);
                    HttpContext.Session.SetString("User", user);
                    return RedirectToPage("/Index");
                }else{
                    return Page();
                }
            }else{
                return Page();
            }
        }  
    }
}
