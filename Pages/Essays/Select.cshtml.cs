using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lingo.Models;
using Lingo.Services;

namespace Lingo.Pages.Essays
{
    public class Result
    {
          public int SampleId { get; set; }
          public string Title { get; set; }
          public string Language { get; set; }
          public string Source { get; set; }
          public string Category { get;set; }
        
    }

    [IgnoreAntiforgeryToken]
    public class SelectModel : PageModel
    {   

        private readonly LingoDbContext context;
        private int UserId;
        public IList<Result> Samples { get; set; }

        // for filter options
        [BindProperty(SupportsGet = true)]
        public string Language { get; set;} 
        [BindProperty(SupportsGet = true)]
        public string Category { get; set;}
        [BindProperty(SupportsGet = true)]
        public List<int> Selected { get; set; }

        // for pagination
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 20;
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public SelectModel(LingoDbContext context, Auth auth)
        {
            this.context = context;
            this.UserId = auth.UserId;
        }

        public async Task<IActionResult> OnGet()
        {

            if (Selected != null && Selected.Count > 0 ){

                await CopySamples();
                return RedirectToPage("./Index");

            }else{
                // query samples not included yet in my essays, perform a left join
                var result = from sample in context.Samples
                join essay in context.Essays on sample.SampleId equals essay.SampleId into tbl
                from t in tbl.DefaultIfEmpty()
                where t.SampleId == null 
                orderby t.SampleId
                select new Result {
                    SampleId = sample.SampleId,
                    Title = sample.Title,
                    Source = sample.Source,
                    Language = sample.Language,
                    Category = sample.Category,
                };
                if (Language != null && Language != "Choose..."){
                    result = result.Where(r => r.Language == Language);
                }
                if (Category != null && Category !="Choose..."){
                    result = result.Where(r => r.Category == Category);
                }
                Count = result.Count();
                // very hard to solve casting error. The query result type cannot be cast implicitly to sample type
                Samples = result.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                return Page();
                
            }
        }
        // copy selected sample essay to My essay
        private async Task CopySamples(){

            foreach (int SampleId in Selected) {
                Console.WriteLine(SampleId);

                var sample = await context.Samples.FindAsync(SampleId);
                Essay essay = new Essay{
                    Title = sample.Title,
                    Text = sample.Text,
                    Language = sample.Language,
                    Source = sample.Source,
                    Category = sample.Category,
                    SampleId = sample.SampleId,
                    Created = DateTime.Now,
                    UserId =  UserId,
                };
                await context.Essays.AddAsync(essay);
                await context.SaveChangesAsync();

            }

        }

    }
}
