using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;

namespace Lingo.Pages.Grammars
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;

        public IList<Grammar> Grammars { get; set; }

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
        public bool ShowNext => CurrentPage < TotalPages;  //property with a getter under the hood that will be called each time you access it. 
        public bool ShowFirst => CurrentPage != 1 ;
        public bool ShowLast => CurrentPage != TotalPages ;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        private int PageRangeSize = 2;

        public List<int> PageRange{
            get{
                int InitialNum = CurrentPage - PageRangeSize;
                int LimitNum = (CurrentPage + PageRangeSize) + 1;
                List<int> arr = new List<int>();
                for (int i = InitialNum; i < LimitNum; i++)
                {
                    arr.Add(i);
                }
                return arr;
            }
        }

        public IndexModel(LingoDbContext context)
        {
            this.context = context;
        }
        public async Task OnGet()
        {
            var result = context.Grammars.AsQueryable();

            if (Language != null && Language != "Choose...")
            {
                result = result.Where(r => r.Language == Language);
            }
            if (Category != null)
            {
                result = result.Where(r => r.Category == Category);
            }
            if (Query != null)
            {
                result = result.Where(r => r.Title.Contains(Query));
            }
            Count = await result.CountAsync();
            Grammars = result.OrderBy(e => e.GrammarId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        } 

    }
}