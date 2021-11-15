using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Samples
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;

        public IList<Sample> Samples { get; set; }

        // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set;} 
        [BindProperty(SupportsGet = true)]
        public string Category { get; set;}
        [BindProperty(SupportsGet = true)]
        public string Query { get; set;}

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
            var result = context.Samples.AsQueryable();

            if (Language != null && Language != "Choose...")
            {
                result = result.Where(r => r.Language == Language);
            }
            if (Category != null && Category != "Choose...")
            {
                result = result.Where(r => r.Category == Category);
            }
            if (Query != null)
            {
                result = result.Where(r => r.Title.Contains(Query));
            }
            Count = await result.CountAsync();
            Samples = result.OrderBy(e => e.SampleId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        //public void OnPost()
        //{
        //    var samples  = from e in context.Samples select e;
        //    if ( Language != "Choose..."){
        //        samples = samples.Where(s => s.Language == Language);
        //    }
        //    if ( Category != "Choose..."){
        //        samples = samples.Where(s => s.Category == Category);
        //    }

        //    // movies = movies.Where(s => s.Title.Contains(SearchString));
        //    Samples = samples.ToList();
        //}

    }
}