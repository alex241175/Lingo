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

    [Route("api/result")]
    [ApiController]
    public class ResultController : Controller
    {

        private readonly LingoDbContext _db;

        public ResultController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/result
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Results.ToListAsync() });
        }

          // POST api/result
        [HttpPost]
        public async Task<IActionResult> Add(Result result)
        {
            if (ModelState.IsValid)
            {
                result.Created = DateTime.Now;
                await _db.Results.AddAsync(result);
                await _db.SaveChangesAsync();

                //return CreatedAtAction("GetUser", new { vocab.UserId }, vocab);
                return Json(result);
            }

            return new JsonResult("Something Went wrong") { StatusCode = 500 };
        }

    
    }
}


