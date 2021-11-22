using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lingo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lingo.Controllers
{

    [Route("api/review")]
    [ApiController]
    public class ReviewController : Controller
    {

        private readonly LingoDbContext _db;
        public Review Review { get; set; }

        public ReviewController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/review
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Reviews.ToListAsync() });
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
    }
}


