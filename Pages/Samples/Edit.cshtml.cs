using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;
using System.Text.RegularExpressions;

namespace Lingo.Pages.Samples
{
    public class EditModel : PageModel
    {
        private readonly LingoDbContext context;

        [BindProperty]
        public Sample Sample { get; set; }
        public string[] Languages = new []{"English","Malay"};
        public string[] Categories = new []{"Easy","Medium","Advanced"};

        public EditModel(LingoDbContext context)
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

        public async Task<IActionResult> OnPost(int sampleId)
        {
            if (Sample.Text != null){
                Sample.Text = Regex.Replace(Sample.Text, @"\r\n?|\n", "<br/>");
            }  
            context.Samples.Update(Sample);
            await context.SaveChangesAsync();

            return RedirectToPage("./Details", new { sampleId = sampleId });
        }
    }
}