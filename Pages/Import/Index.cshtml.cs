using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lingo.Models;

namespace Lingo.Pages.Import
{
    public class IndexModel : PageModel
    {
        private readonly LingoDbContext _db;
        [BindProperty]
        public IFormFile Upload { get; set; }
        public string Message { get; set; }

        private IWebHostEnvironment _environment;

        public IndexModel(IWebHostEnvironment environment, LingoDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        public async Task<IActionResult> OnPost()
        {
            //var file = Path.Combine(_environment.ContentRootPath, "Data", Upload.FileName);
            //using (var fileStream = new FileStream(file, FileMode.Create))
            //{
                //await Upload.CopyToAsync(fileStream); //save file in the path
            //}
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                Upload.CopyTo(stream);
                stream.Position = 0;
                bool IsFirstRow = true;
                int n = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        if (!IsFirstRow){
                            Vocab vocab = new Vocab {
                                Language = reader.GetValue(0).ToString(),
                                Text = reader.GetValue(1).ToString(),
                                Chinese = reader.GetValue(2) != null ? reader.GetValue(2).ToString() : "",
                                Note = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : "",
                                Category = reader.GetValue(4).ToString(),
                                UserId = reader.GetValue(5) != null ? int.Parse(reader.GetValue(5).ToString()) : null,
                                Created = DateTime.Now,
                            };
                            _db.Vocabs.Add(vocab);
                            _db.SaveChanges();
                            n++;
                        }
                        if (IsFirstRow) IsFirstRow = false;
                    }
                }
                Message = "Total records saved into Database : " + n;
            }
            return Page();

        }
    }
}
