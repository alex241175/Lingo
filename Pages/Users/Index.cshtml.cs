using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;
        public IList<User> Users { get; set; }

        public IndexModel(LingoDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            Users = context.Users.ToList();
        }
    }
}