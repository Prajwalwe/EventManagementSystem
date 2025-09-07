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
    public class CollegesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollegesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Colleges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<College>>> GetColleges()
        {
            return await _context.Colleges.ToListAsync();
        }

        // GET: api/Colleges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<College>> GetCollege(int id)
        {
            var college = await _context.Colleges.FindAsync(id);

            if (college == null)
            {
                return NotFound();
            }

            return college;
        }

        // PUT: api/Colleges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollege(int id, College college)
        {
            if (id != college.CollegeId)
            {
                return BadRequest();
            }

            _context.Entry(college).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollegeExists(id))
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

        // POST: api/Colleges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<College>> PostCollege(College college)
        {
            _context.Colleges.Add(college);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollege", new { id = college.CollegeId }, college);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollege(int id)
        {
            var college = await _context.Colleges.FindAsync(id);
            if (college == null)
                return NotFound();

            // Use raw SQL to delete
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Colleges WHERE CollegeId = {0}", id);

            return NoContent();
        }



        private bool CollegeExists(int id)
        {
            return _context.Colleges.Any(e => e.CollegeId == id);
        }
    }
}
