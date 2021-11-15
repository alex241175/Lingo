using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Samples
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
        public Sample Sample { get; set; }

        public DetailsModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> OnGet(int sampleId)
        {
            Sample = await context.Samples.FindAsync(sampleId);
            if (Sample == null)
            {
                return RedirectToPage("../Shared/ErrorNotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int sampleId)
        {
            Sample = await context.Samples.FindAsync(sampleId);
            context.Samples.Remove(Sample);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}