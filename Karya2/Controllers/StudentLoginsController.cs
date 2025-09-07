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
    public class StudentLoginsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentLoginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentLogins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentLogin>>> GetStudentLogins()
        {
            return await _context.StudentLogins.ToListAsync();
        }

        // GET: api/StudentLogins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentLogin>> GetStudentLogin(string id)
        {
            var studentLogin = await _context.StudentLogins.FindAsync(id);

            if (studentLogin == null)
            {
                return NotFound();
            }

            return studentLogin;
        }

        // PUT: api/StudentLogins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentLogin(string id, StudentLogin studentLogin)
        {
            if (id != studentLogin.UserName)
            {
                return BadRequest();
            }

            _context.Entry(studentLogin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentLoginExists(id))
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

        // POST: api/StudentLogins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentLogin>> PostStudentLogin(StudentLogin studentLogin)
        {
            _context.StudentLogins.Add(studentLogin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentLoginExists(studentLogin.UserName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentLogin", new { id = studentLogin.UserName }, studentLogin);
        }

        // DELETE: api/StudentLogins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentLogin(string id)
        {
            var studentLogin = await _context.StudentLogins.FindAsync(id);
            if (studentLogin == null)
            {
                return NotFound();
            }

            _context.StudentLogins.Remove(studentLogin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/StudentLogins/Validate?userName=abc&password=xyz
        [HttpGet("Validate")]
        public async Task<ActionResult<int>> ValidateStudentLogin(string userName, string password)
        {
            var studentLogin = await _context.StudentLogins
                .FirstOrDefaultAsync(s => s.UserName == userName);

            if (studentLogin == null)
            {
                return NotFound("User does not exist");
            }

            if (studentLogin.Password != password)
            {
                return Unauthorized("Invalid password");
            }

            return Ok(studentLogin.StudentId); // return StudentId if valid
        }


        private bool StudentLoginExists(string id)
        {
            return _context.StudentLogins.Any(e => e.UserName == id);
        }
    }
}
