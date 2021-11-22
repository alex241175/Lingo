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
    public class EditModel : PageModel
    {
        private readonly LingoDbContext context;
        private IHostingEnvironment _environment;

        [BindProperty]
        public Grammar Grammar { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public string[] Languages = new []{"English","Malay"};

        public EditModel(LingoDbContext context, IHostingEnvironment environment)
        {
            this.context = context;
            _environment = environment;
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

        public async Task<IActionResult> OnPost(int grammarId)
        {
            
            // upload file
            if (Upload != null){
                var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                Grammar.Filename = Upload.FileName;  
            }
            context.Grammars.Update(Grammar);
            await context.SaveChangesAsync();

            return RedirectToPage("./Details", new { grammarId = grammarId });
        }

        public async Task<IActionResult> OnPostDeleteFile(int grammarId)
        {
            Grammar = await context.Grammars.FindAsync(grammarId);
            // remove file             
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", Grammar.Filename);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }            
            Grammar.Filename = null;
            context.Grammars.Update(Grammar);
            await context.SaveChangesAsync();
            return RedirectToPage("./Edit", new { grammarId = grammarId });
        }
    }
}