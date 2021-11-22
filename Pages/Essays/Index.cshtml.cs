using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;
using Lingo.Services;

namespace Lingo.Pages.Essays
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext context;
        private int UserId;
        public IList<Essay> Essays { get; set; }

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
            var essays  = from e in context.Essays where e.UserId == UserId select e;
            if ( Language != null && Language != "Choose..."){
                essays = essays.Where(e => e.Language == Language);
            }
            if ( Category != null && Category != "Choose..."){
                essays = essays.Where(e => e.Category == Category);
            }
            if ( Query != null){
                essays = essays.Where(e => e.Title.Contains(Query));
            }
            Count = await essays.CountAsync();
            Essays = essays.OrderBy(e => e.EssayId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}