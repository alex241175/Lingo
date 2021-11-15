using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;
using System.Text.RegularExpressions;
using Lingo.Services;

namespace Lingo.Pages.Essays
{
    public class CreateModel : PageModel
    {
        private readonly LingoDbContext context;
        private int UserId;

        [BindProperty]
        public Essay Essay { get; set; }
        public string[] Languages = new []{"English","Malay"};
        public string[] Categories = new []{"Easy","Medium","Advanced"};
        
        public CreateModel(LingoDbContext context, Auth auth)
        {
            this.context = context;
            this.UserId = auth.UserId;
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
            if (Essay.Text != null){
                Essay.Text = Regex.Replace(Essay.Text, @"\r\n?|\n", "<br/>");  
            }
            Essay.Created = DateTime.Now;
            Essay.UserId = UserId;       
            await context.Essays.AddAsync(Essay);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}