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
        [BindProperty]
        public bool RememberMe { get; set; } = false;
        public LoginModel(LingoDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> OnPost(string UserName, string Password, bool RememberMe)
        {
            User User = context.Users.SingleOrDefault(u => u.UserName == UserName);

            if (User != null ){
                if (User.Password == Password){

                    if (RememberMe)
                    {
                        CookieOptions option = new CookieOptions();                        
                        option.Expires = DateTime.Now.AddHours(8);
                        Response.Cookies.Append("UserName", UserName, option);
                    }

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
