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
    public class AttendancesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Attendances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            return await _context.Attendances.ToListAsync();
        }

        // GET: api/Attendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
            {
                return NotFound();
            }

            return attendance;
        }

        // PUT: api/Attendances/Registration/5
        [HttpPut("{registrationId}")]
        public async Task<IActionResult> UpdateAttendance(int registrationId, [FromBody] bool isPresent)
        {
            // Find attendance by RegistrationId
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.RegistrationId == registrationId);

            if (attendance == null)
            {
                return NotFound("Attendance for this registration does not exist.");
            }

            // Update IsPresent
            attendance.IsPresent = isPresent;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(attendance.AttendanceId))
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



        // POST: api/Attendances
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
        {
            // Check if the Registration exists
            var registrationExists = await _context.Registrations
                .AnyAsync(r => r.RegistrationId == attendance.RegistrationId);

            if (!registrationExists)
            {
                return NotFound("Registration does not exist.");
            }

            // Check if attendance for this RegistrationId is already marked
            var alreadyMarked = await _context.Attendances
                .AnyAsync(a => a.RegistrationId == attendance.RegistrationId);

            if (alreadyMarked)
            {
                return Conflict("Attendance already marked for this registration.");
            }

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendance", new { id = attendance.AttendanceId }, attendance);
        }


        // DELETE: api/Attendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendanceExists = await _context.Attendances.AnyAsync(a => a.AttendanceId == id);
            if (!attendanceExists)
            {
                return NotFound();
            }

            // Use raw SQL to delete Attendance
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Attendances WHERE AttendanceId = {0}", id);

            return NoContent();
        }


        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceId == id);
        }
    }
}
