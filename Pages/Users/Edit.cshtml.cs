using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;

namespace Lingo.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly LingoDbContext context;

        [BindProperty]
        new public User User { get; set; }

        public EditModel(LingoDbContext context)
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

        public async Task<IActionResult> OnPost(int userId)
        {
            context.Users.Update(User);
            await context.SaveChangesAsync();

            return RedirectToPage("./Details", new { userId = userId });
        }
    }
}