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
    public class CreateModel : PageModel
    {
        private readonly LingoDbContext context;

        [BindProperty]
        new public User User { get; set; }

        public CreateModel(LingoDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }            
            await context.Users.AddAsync(User);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}