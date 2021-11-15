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
    public class CreateModel : PageModel
    {
        private readonly LingoDbContext context;
        // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        [BindProperty]
        public Sample Sample { get; set; }
        public string[] Languages = new []{"English","Malay"};
        public string[] Categories = new []{"Easy","Medium","Advanced"};
        
        public CreateModel(LingoDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            Sample = new Sample();
            if (Language != null)
            {
                Sample.Language = Language;
            }
            if (Category != null)
            {
                Sample.Category = Category;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Sample.Text != null){
                Sample.Text = Regex.Replace(Sample.Text, @"\r\n?|\n", "<br/>");
            }  
            Sample.Created = DateTime.Now;       
            await context.Samples.AddAsync(Sample);
            await context.SaveChangesAsync();

            return Redirect($"~/Samples/Index?Language={Language}&Category={Category}");
        }
    }
}