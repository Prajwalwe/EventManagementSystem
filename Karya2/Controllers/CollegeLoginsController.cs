using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kriya2.Models.Data;
using Kriya2.Models.Entities;

namespace Karya2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeLoginsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollegeLoginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CollegeLogins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollegeLogin>>> GetCollegeLogin()
        {
            return await _context.CollegeLogin.ToListAsync();
        }

        // GET: api/CollegeLogins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CollegeLogin>> GetCollegeLogin(string id)
        {
            var collegeLogin = await _context.CollegeLogin.FindAsync(id);

            if (collegeLogin == null)
            {
                return NotFound();
            }

            return collegeLogin;
        }

        // PUT: api/CollegeLogins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollegeLogin(string id, CollegeLogin collegeLogin)
        {
            if (id != collegeLogin.UserName)
            {
                return BadRequest();
            }

            _context.Entry(collegeLogin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollegeLoginExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CollegeLogins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CollegeLogin>> PostCollegeLogin(CollegeLogin collegeLogin)
        {
            _context.CollegeLogin.Add(collegeLogin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CollegeLoginExists(collegeLogin.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCollegeLogin", new { id = collegeLogin.UserName }, collegeLogin);
        }

        // DELETE: api/CollegeLogins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollegeLogin(string id)
        {
            var collegeLogin = await _context.CollegeLogin.FindAsync(id);
            if (collegeLogin == null)
            {
                return NotFound();
            }

            _context.CollegeLogin.Remove(collegeLogin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollegeLoginExists(string id)
        {
            return _context.CollegeLogin.Any(e => e.UserName == id);
        }
    }
}
