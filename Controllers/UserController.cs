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
    [Route("api/User")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly LingoDbContext _db;

        public UserController(LingoDbContext db)
        {
            _db = db;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = _db.Users.ToList();
            return Json(new { data = await _db.Users.ToListAsync() });
        }

        // GET api/user/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST api/user
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            if (ModelState.IsValid)
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                //return CreatedAtAction("GetUser", new { user.UserId }, user);
                return Json(user);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        // PUT api/user
        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            if  (user == null)
                return NotFound();

            _db.Update(user);
            await _db.SaveChangesAsync();
            return Json(user);
        }

        // DELETE api/user/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Remove(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Ok(user);
        }
    }
}
