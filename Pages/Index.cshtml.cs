using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Lingo.Models;
using Newtonsoft.Json;

namespace Lingo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly LingoDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, LingoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            if (Request.Cookies["UserName"] != null){
                string userName = Request.Cookies["UserName"];
                User User = _context.Users.SingleOrDefault(u => u.UserName == userName);
                var user = JsonConvert.SerializeObject(User);
                HttpContext.Session.SetString("User", user);
            }
        }
    }
}
