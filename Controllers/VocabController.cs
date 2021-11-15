using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lingo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lingo.Controllers
{
    [Route("api/vocab")]
    [ApiController]
    public class VocabController : Controller
    {

        private readonly LingoDbContext _db;

        public VocabController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/vocab
        [HttpGet]
        public async Task<IActionResult> GetAll(string Language, int CurrentPage, int PageSize)
        {
            var vocabs = _db.Vocabs.AsQueryable();
            // filter by language
            if (!string.IsNullOrWhiteSpace(Language)){
                vocabs = vocabs.Where(v => v.Language == Language);
            }
            var count = vocabs.Count();
            // filter by page number
            vocabs = vocabs.OrderBy(v=>v.VocabId).Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            return Json(new { data = await vocabs.ToListAsync(), count = count });
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
            var vocab = await _db.Vocabs.FirstOrDefaultAsync(v => v.VocabId == VocabId);
            if (vocab == null)
                return NotFound();

            return Ok(vocab);
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

        // POST api/vocab
        [HttpPost]
        public async Task<IActionResult> Add(Vocab vocab)
        {
            if (ModelState.IsValid)
            {
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
