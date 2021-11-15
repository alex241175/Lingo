using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly LingoDbContext context;
        new public User User { get; set; }

        public DetailsModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> OnGet(int userId)
        {
            User = await context.Users.FindAsync(userId);
            if (User == null)
            {
                return RedirectToPage("../Shared/ErrorNotFound");
            }

            return Page();
        }
    }
}