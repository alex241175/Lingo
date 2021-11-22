using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;
using Lingo.Services;

namespace Lingo.Pages.Results
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;
        private int UserId;
        private Result Result { get; set; }
        public IList<Result> Results { get; set; }

        // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set;} 
   
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

        public IndexModel(LingoDbContext context, Auth auth)
        {
            this.context = context;
            this.UserId = auth.UserId;
        }
        public async Task OnGet()
        {
            var result = from r in context.Results where r.UserId == UserId select r ;

            if (Language != null && Language != "Choose...")
            {
                result = result.Where(r => r.Language == Language);
            }

            Count = await result.CountAsync();
            Results = result.OrderByDescending(e => e.ResultId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
        public async Task<IActionResult> OnPostDelete(int resultId)
        {
            Result = await context.Results.FindAsync(resultId);
            context.Results.Remove(Result);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}