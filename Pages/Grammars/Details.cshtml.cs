using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Grammars
{
    public class DetailsModel : PageModel
    {
        private readonly LingoDbContext context;
         // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set;} 
        [BindProperty(SupportsGet = true)]
        public string Category { get; set;}
        [BindProperty(SupportsGet = true)]
        public string Query { get; set;}
        public Grammar Grammar { get; set; }

        public DetailsModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> OnGet(int grammarId)
        {
            Grammar = await context.Grammars.FindAsync(grammarId);
            if (Grammar == null)
            {
                return RedirectToPage("../Shared/ErrorNotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int grammarId)
        {
            Grammar = await context.Grammars.FindAsync(grammarId);
            context.Grammars.Remove(Grammar);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}