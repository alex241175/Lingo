using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Essays
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;
        public IList<Essay> Essays { get; set; }

        // for filter options
        [BindProperty]
        public string Language { get; set;} 
        [BindProperty]
        public string Category { get; set;}

        // for pagination
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public IndexModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task OnGet()
        {
            Count = await context.Essays.CountAsync();
            Essays = context.Essays.OrderBy(e => e.EssayId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public void OnPost()
        {
            var essays  = from e in context.Essays select e;
            if ( Language != "Choose..."){
                essays = essays.Where(e => e.Language == Language);
            }
            if ( Category != "Choose..."){
                essays = essays.Where(e => e.Category == Category);
            }

            // movies = movies.Where(s => s.Title.Contains(SearchString));
            Essays = essays.ToList();
        }

    }
}