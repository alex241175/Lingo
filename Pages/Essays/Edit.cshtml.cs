using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;
using System.Text.RegularExpressions;

namespace Lingo.Pages.Essays
{
    public class EditModel : PageModel
    {
        private readonly LingoDbContext context;

        [BindProperty]
        public Essay Essay { get; set; }
        public string[] Languages = new []{"English","Malay"};
        public string[] Categories = new []{"Easy","Medium","Advanced"};

        public EditModel(LingoDbContext context)
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

        public async Task<IActionResult> OnPost(int essayId)
        {
            if (Essay.Text != null){
                Essay.Text = Regex.Replace(Essay.Text, @"\r\n?|\n", "<br/>");  
            }
            context.Essays.Update(Essay);
            await context.SaveChangesAsync();

            return RedirectToPage("./Details", new { essayId = essayId });
        }
    }
}