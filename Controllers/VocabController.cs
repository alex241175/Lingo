using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lingo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lingo.Services;
using System.Text.RegularExpressions;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lingo.Controllers
{
    [Route("api/vocab")]
    [ApiController]
    public class VocabController : Controller
    {

        private readonly LingoDbContext _db;
        private int UserId;

        public class Result
        {
            public int VocabId {get; set;}
            public string Language { get; set; }
            public string Text { get; set; }
            public string Chinese { get; set; }       
            public string Note {get;set;}
            public string Category { get; set; }  
            public int? EssayId { get; set; }   
            public int? UserId { get; set; }  
            public bool IsReview { get; set; }
            public int? ReviewId { get; set; }     
    
        }

        public VocabController(LingoDbContext db, Auth auth)
        {
            _db = db;
            UserId = auth.UserId;
        }

        // GET: api/vocab
        [HttpGet]
        public async Task<IActionResult> GetAll(string Language, string Category, int CurrentPage, int PageSize, int UserId)
        {
            // based on this sql
            // select vocabs.*, r.ReviewId from vocabs left join (select * from reviews where UserId = 15) AS r on vocabs.VocabId = r.VocabId 
            
            // get data source with review information if any

            var vocabs = from v in _db.Vocabs
                join r in ( from d in _db.Reviews where d.UserId == UserId select d ) 
                on v.VocabId equals r.VocabId into tbl
                from t in tbl.DefaultIfEmpty()
                orderby v.VocabId
                select new Result {
                    VocabId = v.VocabId,
                    Language = v.Language,
                    Text = v.Text,
                    Chinese = v.Chinese,
                    Note = v.Note,
                    Category = v.Category,
                    EssayId = v.EssayId,
                    UserId = v.UserId,
                    IsReview = t.VocabId != null ? true : false,
                    ReviewId = t.ReviewId                    
                };

             //var vocabs = from v in _db.Vocabs select v;
            // filter by language
            if (!string.IsNullOrWhiteSpace(Language)){
                vocabs = vocabs.Where(v => v.Language == Language);
            }
            // filter by category
            if (!string.IsNullOrWhiteSpace(Category)){
                vocabs = vocabs.Where(v => v.Category == Category);
                if (Category == "User"){
                    vocabs = vocabs.Where(v => v.UserId == UserId);
                }
            }   
            var count = vocabs.Count();
            // filter by page number
            var Results = await vocabs.OrderBy(v=> v.VocabId).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToListAsync();

            return Json(new { data = Results, count = count });
        }

        // GET: api/vocab/review
        [Route("~/api/vocab/review")]
        [HttpGet]
        public async Task<IActionResult> GetReview(string Language, int CurrentPage, int PageSize, int UserId)
        {
            // based on this sql
            // select vocabs.*, r.ReviewId from vocabs left join (select * from reviews where UserId = 15) AS r on vocabs.VocabId = r.VocabId 
            
            // get data source with review information if any

            var vocabs = from v in _db.Vocabs
                join r in ( from d in _db.Reviews where d.UserId == UserId select d ) 
                on v.VocabId equals r.VocabId into tbl
                from t in tbl.DefaultIfEmpty()
                orderby v.VocabId
                select new Result {
                    VocabId = v.VocabId,
                    Language = v.Language,
                    Text = v.Text,
                    Chinese = v.Chinese,
                    Note = v.Note,
                    Category = v.Category,
                    EssayId = v.EssayId,
                    UserId = v.UserId,
                    IsReview = t.VocabId != null ? true : false,
                    ReviewId = t.ReviewId                    
                };

             //var vocabs = from v in _db.Vocabs select v;
            // filter by language
            if (!string.IsNullOrWhiteSpace(Language)){
                vocabs = vocabs.Where(v => v.Language == Language);
            }   
            vocabs = vocabs.Where(v => v.IsReview == true).OrderBy(v=> v.ReviewId);
            
            var count = vocabs.Count();
            // filter by page number
            var Results = await vocabs.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToListAsync();

            return Json(new { data = Results, count = count });
        }

        // GET: api/vocab/typeahead
        [Route("~/api/vocab/typeahead")]
        [HttpGet]
        public async Task<IActionResult> GetTypeAhead(string Text, string Language, int UserId)
        {
            var vocabs = from v in _db.Vocabs 
            where v.Language == Language 
            where v.Text.ToLower().Contains(Text)
            where v.Category == "User"
            orderby v.Text 
            select new {
                id = v.VocabId,
                name = v.Text
            };
            return Json( await vocabs.ToListAsync());
        }

          // GET: api/mcq
        [Route("~/api/vocab/mcq")]
        [HttpGet]
        public async Task<IActionResult> GetMcq(string Language, string Category, int CurrentPage, int PageSize, int UserId)
        {
            var vocabs = from v in _db.Vocabs
            join r in ( from d in _db.Reviews where d.UserId == UserId select d ) 
            on v.VocabId equals r.VocabId into tbl
            from t in tbl.DefaultIfEmpty()
            orderby v.VocabId
            select new Result{
                VocabId = v.VocabId,
                Language = v.Language,
                Text = v.Text,
                Chinese = v.Chinese,
                Note = v.Note,
                Category = v.Category,
                EssayId = v.EssayId,
                UserId = v.UserId,
                IsReview = t.VocabId != null ? true : false 
            };
            
            //var vocabs = from v in _db.Vocabs select v;
            // filter by language
            if (!string.IsNullOrWhiteSpace(Language)){
                vocabs = vocabs.Where(v => v.Language == Language);
            }
            // filter by category
            if (!string.IsNullOrWhiteSpace(Category)){
                vocabs = vocabs.Where(v => v.Category == Category);
                if (Category == "User"){
                    vocabs = vocabs.Where(v => v.UserId == UserId);
                }
            }
            // filter by page number
            vocabs = vocabs.OrderBy(v=>v.VocabId).Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            var count = vocabs.Count();

            // get random records
            var randomList = _db.Vocabs.AsEnumerable().Where(v=> v.Category != "Phrasal Verb").OrderBy(v => Guid.NewGuid()).Take(count*3);

            return Json(new { data = await vocabs.ToListAsync(), randomList = randomList.ToList() });
        }

         // GET: api/vocab/essay/5
        [Route("~/api/vocab/essay/{EssayId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllByEssayId(int EssayId )
        {
            return Json(new { data = await _db.Vocabs.Where(v => v.EssayId == EssayId).ToListAsync() });
        }

        // GET api/vocab/5
        [HttpGet("{VocabId}")]
        public async Task<IActionResult> Get(int VocabId)
        {
            
            var vocab = from v in _db.Vocabs
                join r in ( from d in _db.Reviews where d.UserId == UserId select d ) 
                on v.VocabId equals r.VocabId into tbl
                from t in tbl.DefaultIfEmpty()
                where v.VocabId == VocabId
                select new Result{
                    VocabId = v.VocabId,
                    Language = v.Language,
                    Text = v.Text,
                    Chinese = v.Chinese,
                    Note = v.Note,
                    Category = v.Category,
                    EssayId = v.EssayId,
                    UserId = v.UserId,
                    IsReview = t.VocabId != null ? true : false 
                };

            // var vocab = await _db.Vocabs.FirstOrDefaultAsync(v => v.VocabId == VocabId);

            return Ok(await vocab.FirstOrDefaultAsync());
        }

        // GET api/vocab/translate/text
        [Route("~/api/vocab/translate/{sourceText}")]
        [HttpGet]
        public async Task<IActionResult> Translate(string sourceText)
        {
            using var client = new HttpClient();
            string api_key = "AIzaSyDDTxvvy4APx1xLDuMYHc0BU2HMd8lBVHI";
            string target = "zh";
            //string source = "en";  // Malay - ms
            string url = $"https://translation.googleapis.com/language/translate/v2?key={api_key}&q={sourceText}&target={target}";
            var result = await client.GetStringAsync(url);
            return Json(result);
        }

            // GET api/vocab/dict/text
        [Route("~/api/vocab/dict/{sourceText}")]  
        [HttpGet]
        public async Task<IActionResult> Dict(string sourceText)
        {
            using var client = new HttpClient();
            // https://apii.dict.cn/mini.php?q=miss
            string url = $"https://apii.dict.cn/mini.php?q={sourceText}";
            var result = await client.GetStringAsync(url);

            // var result = @"<span class='g b'>Define <span class='k'>miss</span> :</span><span class='p'> [mɪs]</span><br>
            // <div id=""e"">n.[M-]小姐<br>vt.想念<br>vt.错过；漏掉</div>
            // <br>
            // <div class='t'>例句与用法:</div>
            // <div id=""s""><i>1</i>. Miss Williams can read and write French very well.<br>威廉斯小姐能够很自如地用法语看书和写东西。<br><i>2</i>. I do <em><em>miss</em></em>the children. The house seems as silent as the tomb without them.<br>我真想念孩子们。他们不在家里显得太沉静了。<br>
            // <i>3</i>. It's the chance of a lifetime. You shouldn't <em><em>miss</em></em> it.<br>这是一生中难得的机会，你不应错过。<br><i>4</i>. He listened attentively so as not to <em><em>miss</em></em> a single word .<br>
            //  他不想漏掉一个字，所以很用心的听了。<br></div>
            //  <br>
            //  <div class='t'>词形变化:</div><div id='t'>过去式: <i>missed</i> 过去分词: <i>missed</i> 
            //  现在分词: <i>missing</i> 第三人称单数: <i>misses</i> </div></div><br>";
             Console.WriteLine("Testing...");
             string patternE = "<div id=\"e\">(.*?)</div>";   // definition
             string patternS = "<div id=\"s\">(.*?)</div>";   // usage sentence
             string patternT = "<div id=\"t\">(.*?)</div>";   // tenses

             Match m = Regex.Match(result,patternE,RegexOptions.IgnoreCase);
             // Console.WriteLine(m.Value);
             Match m2 = Regex.Match(result,patternS,RegexOptions.IgnoreCase);
             //Console.WriteLine(m2.Value);
             Match m3 = Regex.Match(result,patternT,RegexOptions.IgnoreCase);
             //Console.WriteLine(m2.Value);

            return Json(new { e = m.Value, s = m2.Value, t = m3.Value });

            //string result = "<div>This is a test</div><div class=\"something\">This is ANOTHER test</div>";
            //string pattern = "<div.*?>(.*?)<\\/div>";
            // Create a Regex  
            //     MatchCollection matches = Regex.Matches(result, pattern, RegexOptions.IgnoreCase);
            //    if (matches.Count > 0){
            //         foreach (Match m in matches){
            //             Console.WriteLine("Inner DIV: {0}", m.Groups[1]);
            //         }
            //    }
            //return Json(result);
        }
    

        // POST api/vocab
        [HttpPost]
        public async Task<IActionResult> Add(Vocab vocab)
        {
            if (ModelState.IsValid)
            {
                vocab.Created = DateTime.Now;
                await _db.Vocabs.AddAsync(vocab);
                await _db.SaveChangesAsync();

                //return CreatedAtAction("GetUser", new { vocab.UserId }, vocab);
                return Json(vocab);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        // PUT api/vocab
        [HttpPut]
        public async Task<IActionResult> Update(Vocab vocab)
        {
            if  (vocab == null)
                return NotFound();

            _db.Update(vocab);
            await _db.SaveChangesAsync();
            return Json(vocab);
        }

        // DELETE api/vocab/5
        [HttpDelete("{vocabId}")]
        public async Task<IActionResult> Remove(int vocabId)
        {
            var vocab = await _db.Vocabs.FirstOrDefaultAsync(v => v.VocabId == vocabId);

            if (vocab == null)
                return NotFound();

            _db.Vocabs.Remove(vocab);
            await _db.SaveChangesAsync();

            return Ok(vocab);
        }
       
    }
}
