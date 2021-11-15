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

    [Route("api/sample")]
    [ApiController]
    public class SampleController : Controller
    {

        private readonly LingoDbContext _db;

        public SampleController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/sample
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Samples.ToListAsync() });
        }

        // GET api/sample/5
        [HttpGet("{SampleId}")]
        public async Task<IActionResult> Get(int SampleId)
        {
            var sample = await _db.Samples.FindAsync(SampleId);
            if (sample == null)
                return NotFound();

            return Ok(sample);
        }       
    }
}


