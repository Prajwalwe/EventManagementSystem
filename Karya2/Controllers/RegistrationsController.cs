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
    public class RegistrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations()
        {
            return await _context.Registrations.ToListAsync();
        }

        // GET: api/Registrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Registration>> GetRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);

            if (registration == null)
            {
                return NotFound();
            }

            return registration;
        }

        // POST: api/Registrations
        [HttpPost]
        public async Task<ActionResult<Registration>> PostRegistration(Registration registration)
        {
            // Validate Student
            var studentExists = await _context.Students.AnyAsync(s => s.StudentId == registration.StudentId);
            if (!studentExists)
            {
                return BadRequest("Student does not exist.");
            }

            // Validate Event
            var eventExists = await _context.Events.AnyAsync(e => e.EventId == registration.EventId);
            if (!eventExists)
            {
                return BadRequest("Event does not exist.");
            }

            // Check if this student already registered for this event
            var alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.StudentId == registration.StudentId && r.EventId == registration.EventId);

            if (alreadyRegistered)
            {
                return Conflict("Student is already registered for this event.");
            }

            // Add registration
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistration", new { id = registration.RegistrationId }, registration);
        }


        // DELETE: api/Registrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            // Check if the registration exists
            var registrationExists = await _context.Registrations.AnyAsync(r => r.RegistrationId == id);
            if (!registrationExists)
            {
                return NotFound();
            }

            // Delete using raw SQL; the Attendance trigger will handle dependent records
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Registrations WHERE RegistrationId = {0}", id);

            return NoContent();
        }


        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.RegistrationId == id);
        }
    }
}
