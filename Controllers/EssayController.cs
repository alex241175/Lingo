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

    [Route("api/essay")]
    [ApiController]
    public class EssayController : Controller
    {

        private readonly LingoDbContext _db;

        public EssayController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/essay
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Essays.ToListAsync() });
        }

        // GET api/essay/5
        [HttpGet("{EssayId}")]
        public async Task<IActionResult> Get(int EssayId)
        {
            var essay = await _db.Essays.FindAsync(EssayId);
            if (essay == null)
                return NotFound();

            return Ok(essay);
        }

        // PUT api/essay
        [HttpPut("{EssayId}")]
        public async Task<IActionResult> Update(int EssayId, [FromBody] object content)
        {
            var Essay = await _db.Essays.FindAsync(EssayId);
            Essay.Text = content.ToString();
            _db.Update(Essay);
            await _db.SaveChangesAsync();
            return Json(Essay);
        }
       
    }
}


