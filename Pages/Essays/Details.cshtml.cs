using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Essays
{
    public class DetailsModel : PageModel
    {
        private readonly LingoDbContext context;
        public Essay Essay { get; set; }

        public DetailsModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> OnGet(int essayId)
        {
            Essay = await context.Essays.FindAsync(essayId);
            if (Essay == null)
            {
                return RedirectToPage("../Shared/ErrorNotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int essayId)
        {
            Essay = await context.Essays.FindAsync(essayId);
            context.Essays.Remove(Essay);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}