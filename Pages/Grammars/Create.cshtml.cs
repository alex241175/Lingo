using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Lingo.Pages.Grammars
{
    public class CreateModel : PageModel
    {
        private readonly LingoDbContext context;
        private IHostingEnvironment _environment;
        // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        [BindProperty]
        public Grammar Grammar { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public string[] Languages = new []{"English","Malay"};
        public string[] Categories = new []{"Easy","Medium","Advanced"};
        
        public CreateModel(LingoDbContext context, IHostingEnvironment environment)
        {
            this.context = context;
            _environment = environment;
        }
        public void OnGet()
        {
            Grammar = new Grammar();
            if (Language != null)
            {
                Grammar.Language = Language;
            }
            if (Category != null)
            {
                Grammar.Category = Category;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
         
            // upload file
            var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            Grammar.Created = DateTime.Now;
            Grammar.Filename = Upload.FileName;       
            await context.Grammars.AddAsync(Grammar);
            await context.SaveChangesAsync();

            return Redirect($"~/Grammars/Index?Language={Language}&Category={Category}");
        }
    }
}