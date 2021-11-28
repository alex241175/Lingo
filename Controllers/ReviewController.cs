using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lingo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lingo.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lingo.Controllers
{

    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {

        private readonly LingoDbContext _db;
        public Review Review { get; set; }
        public int? UserId { get; set; }  
        public class Result
        {
            public int VocabId {get; set;}
            public string Language { get; set; }
            public string Text { get; set; }
            public string Chinese { get; set; }       
            public string Note {get;set;}
            public string Category { get; set; }  
            public string EssayIds { get; set; }   
            public int? UserId { get; set; }  
            public bool IsReview { get; set; }
            public int? ReviewId { get; set; }  
            public int? Index { get; set;}   
        }

        public ReviewController(LingoDbContext db, Auth auth )
        {
            _db = db;
            UserId = auth.UserId;
        }

        // GET: api/review
        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     return Json(new { data = await _db.Reviews.ToListAsync() });
        // }
             // GET: api/vocab/review
        [HttpGet]
        public async Task<IActionResult> GetAll(string Language, int CurrentPage, int PageSize, int UserId)
        {
            // based on this sql
            // select vocabs.*, r.ReviewId from vocabs left join (select * from reviews where UserId = 15) AS r on vocabs.VocabId = r.VocabId 
            
            // get data source with review information if any

            var ReviewSetting = await _db.ReviewSettings.Where(x => x.Language == Language).FirstOrDefaultAsync();
            int i = 1;
            var vocabs = from v in _db.Vocabs 
                where v.Language == Language
                where v.Category == ReviewSetting.VocabsCategory
                where v.UserId == UserId
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
                    UserId = v.UserId,
                    IsReview = t.VocabId != null ? true : false,
                    ReviewId = t.ReviewId,
                };

            var Total = vocabs.Count();  // total vocabs in the category
             //var vocabs = from v in _db.Vocabs select v;
            // filter by language
            // if (!string.IsNullOrWhiteSpace(Language)){
            //     vocabs = vocabs.Where(v => v.Language == Language);
            // }   
            vocabs = vocabs.Where(v => v.IsReview == true).OrderBy(v=> v.ReviewId);
            
            var Count = vocabs.Count();
            // filter by page number
            var Results = await vocabs.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToListAsync();

            // calculate the position of last review vocab in the category

            int Position = 0;
            if (Results.Any()){
                var LastVocabId = Results.Last().VocabId;

                var LastVocabCount = (from v in _db.Vocabs
                where v.Category == ReviewSetting.VocabsCategory
                where v.UserId == UserId
                where v.Language == Language 
                where v.VocabId < LastVocabId
                select v).Count();

                Position = (int)Math.Ceiling(decimal.Divide(LastVocabCount, Total) * 100);
            }

            return Json(new { data = Results, count = Count, position = Position });
        }

        // GET: api/review/refresh    Initialize review vocab for the day
        [Route("~/api/review/refresh")]
        [HttpGet]
        public async Task<IActionResult> Refresh(string Language)
        {
            // step 1 - remove those vocabs passed (equal to PassedThreshold)
            var ReviewSetting = await _db.ReviewSettings.Where(x => x.Language == Language).FirstOrDefaultAsync();

            var passedCollection = _db.Reviews.Where(x => x.Passed >= ReviewSetting.PassedThreshold).ToList();
            if(passedCollection.Any())            {
                _db.Reviews.RemoveRange(passedCollection);         
                await _db.SaveChangesAsync();
            }

            // find out how many vacancy left after removing
            var review = from v in _db.Vocabs 
                where v.Language == Language
                where v.Category == ReviewSetting.VocabsCategory
                where v.UserId == UserId
                join r in ( from d in _db.Reviews where d.UserId == UserId select d ) 
                on v.VocabId equals r.VocabId 
                select v;

            var Count = review.ToList().Count;
            var NewVocabsCount = ReviewSetting.VocabsPerDay - Count;

            // pull new vocabs from Vocabs Table
            var vocabs = from v in _db.Vocabs 
            where v.Language == Language
            where v.UserId == UserId
            where v.Category == ReviewSetting.VocabsCategory 
            where v.VocabId > ReviewSetting.LastVocabId
            orderby v.VocabId            
            select new Review{
                VocabId = v.VocabId,
                UserId = v.UserId,
                Passed = 0,
            };
            var newVocabsCollection = await vocabs.Take(NewVocabsCount).ToListAsync();

            if(newVocabsCollection.Any()){
                // add to Reviews Table            
                _db.Reviews.AddRange(newVocabsCollection);         
                await _db.SaveChangesAsync();
                
                // update last VocabsId to ReviewSetting
                ReviewSetting.LastVocabId = (int)newVocabsCollection.Last().VocabId;
                _db.ReviewSettings.Update(ReviewSetting);
                await _db.SaveChangesAsync();

            }
            return Json(new { data = NewVocabsCount });
        }

           // GET: api/review/mcq
        [Route("~/api/review/mcq")]
        [HttpGet]
        public async Task<IActionResult> GetMcq(string Language, int CurrentPage, int PageSize, int UserId)
        {
            var ReviewSetting = await _db.ReviewSettings.Where(x => x.Language == Language).FirstOrDefaultAsync();

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
            vocabs = vocabs.Where(v => v.Category == ReviewSetting.VocabsCategory) ;
            if (ReviewSetting.VocabsCategory == "User"){
                vocabs = vocabs.Where(v => v.UserId == UserId);
            }
            vocabs = vocabs.Where(v => v.IsReview == true);

            // filter by page number
            vocabs = vocabs.OrderBy(v=>v.ReviewId).Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            var count = vocabs.Count();

            // get random records
            var randomList = _db.Vocabs.AsEnumerable().Where(v=> v.Category != "Phrasal Verb").OrderBy(v => Guid.NewGuid()).Take(count*3);

            return Json(new { data = await vocabs.ToListAsync(), randomList = randomList.ToList() });
        }

        // GET: api/review/match
        [Route("~/api/review/match")]
        [HttpGet]
        public async Task<IActionResult> GetMatch(string Language, int CurrentPage, int PageSize, int UserId)
        {
            
            var ReviewSetting = await _db.ReviewSettings.Where(x => x.Language == Language).FirstOrDefaultAsync();

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
            vocabs = vocabs.Where(v => v.Category == ReviewSetting.VocabsCategory) ;
            if (ReviewSetting.VocabsCategory == "User"){
                vocabs = vocabs.Where(v => v.UserId == UserId);
            }
            vocabs = vocabs.Where(v => v.IsReview == true);

            // filter by page number
            vocabs = vocabs.OrderBy(v=> v.ReviewId).Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            var count = vocabs.Count();

            return Json(new { data = await vocabs.ToListAsync()});
        }

        
        [HttpGet]
        [Route("~/api/review/passed")]
        public async void Passed(int ReviewId)
        {
            var Review = await _db.Reviews.FindAsync(ReviewId);
            Review.Passed = Review.Passed + 1 ;
            _db.Reviews.Update(Review);
            await _db.SaveChangesAsync();
            
        }
        
        // POST api/review
        [HttpPost]
        public async Task<IActionResult> Add(Review review)
        {
            if (ModelState.IsValid)
            {
                await _db.Reviews.AddAsync(review);
                await _db.SaveChangesAsync();

                //return CreatedAtAction("GetUser", new { vocab.UserId }, vocab);
                return Json(review);
            }

            return new JsonResult("Something Went wrong") { StatusCode = 500 };
        }

        // DELETE api/review/11
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
                Review = await _db.Reviews.FindAsync(reviewId);
                _db.Reviews.Remove(Review);
                await _db.SaveChangesAsync();
                return Ok("Done");
        }

        // DELETE api/review/bulk
        [Route("~/api/review/bulk")] 
        [HttpDelete]
        public async Task<IActionResult> DeleteAll(IEnumerable<int> reviewIds)
        {
            var existing = _db.Reviews.Where(x => reviewIds.Contains(x.ReviewId)).ToList();
            if(existing.Any())            {
                _db.Reviews.RemoveRange(existing);         
                await _db.SaveChangesAsync();
            }
            return Ok("done");
        }

        // GET api/review/setting
        [Route("~/api/review/setting")]
        [HttpGet] 
        public async Task<IActionResult> GetReviewSetting(string Language)
        {
            var result = await _db.ReviewSettings.FirstOrDefaultAsync(u => u.Language == Language);
            return  Json(new { data = result});
        }

        // POST api/review/setting
        [Route("~/api/review/setting")]
        [HttpPut]
        public async Task<IActionResult> UpdateReviewSetting(ReviewSetting ReviewSetting)
        {

            _db.ReviewSettings.Update(ReviewSetting);
            await _db.SaveChangesAsync();
            return Json(ReviewSetting);
        }
    }
}


