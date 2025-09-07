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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        // PUT: api/Students/5
[HttpPut("{id}")]
public async Task<IActionResult> PutStudent(int id, Student student)
{
    if (id != student.StudentId)
    {
        return BadRequest();
    }

    // Check if the provided CollegeId exists
    var collegeExists = await _context.Colleges.AnyAsync(c => c.CollegeId == student.CollegeId);
    if (!collegeExists)
    {
        return BadRequest("College does not exist");
    }

    _context.Entry(student).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!StudentExists(id))
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

        //// POST: api/Students
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Student>> PostStudent(Student student)
        //{
        //    _context.Students.Add(student);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        //}
        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Check if the provided CollegeId exists
            var collegeExists = await _context.Colleges.AnyAsync(c => c.CollegeId == student.CollegeId);
            if (!collegeExists)
            {
                return BadRequest("College does not exist");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Students/RegistrationsCount/5
        [HttpGet("RegistrationsCount/{studentId}")]
        public async Task<ActionResult<int>> GetStudentRegistrationsCount(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            if (!studentExists)
                return NotFound("Student does not exist.");

            var registrationCount = await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .CountAsync();

            return Ok(registrationCount);
        }

        // GET: api/Students/EventsAttendedCount/5
        [HttpGet("EventsAttendedCount/{studentId}")]
        public async Task<ActionResult<int>> GetStudentEventsAttendedCount(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            if (!studentExists)
                return NotFound("Student does not exist.");

            // Count events where the student was marked present
            var eventsAttendedCount = await (from a in _context.Attendances
                                             join r in _context.Registrations
                                             on a.RegistrationId equals r.RegistrationId
                                             where r.StudentId == studentId && a.IsPresent
                                             select a).CountAsync();

            return Ok(eventsAttendedCount);
        }

        // GET: api/Students/TopActive
        [HttpGet("TopActive")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopActiveStudents()
        {
            // Get students with their attended events count
            var topStudents = await _context.Students
                .Select(s => new
                {
                    s.StudentId,
                    s.Name,
                    EventsAttendedCount = _context.Attendances
                        .Join(_context.Registrations,
                              a => a.RegistrationId,
                              r => r.RegistrationId,
                              (a, r) => new { a, r })
                        .Where(ar => ar.r.StudentId == s.StudentId && ar.a.IsPresent)
                        .Count(),
                    AttendedEvents = _context.Attendances
                        .Join(_context.Registrations,
                              a => a.RegistrationId,
                              r => r.RegistrationId,
                              (a, r) => new { a, r })
                        .Where(ar => ar.r.StudentId == s.StudentId && ar.a.IsPresent)
                        .Join(_context.Events,
                              ar => ar.r.EventId,
                              e => e.EventId,
                              (ar, e) => new { e.EventId, e.Title, e.StartDateTime })
                        .ToList()
                })
                .OrderByDescending(s => s.EventsAttendedCount)
                .Take(3)
                .ToListAsync();

            if (!topStudents.Any())
                return NotFound("No attendance data found.");

            return Ok(topStudents);
        }

        // GET: api/Students/UpcomingEvents/5
        [HttpGet("UpcomingEvents/{studentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUpcomingEventsForStudent(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.StudentId == studentId);
            if (!studentExists)
                return NotFound("Student does not exist.");

            var currentDate = DateTime.Now;

            var upcomingEvents = await (from r in _context.Registrations
                                        join e in _context.Events
                                        on r.EventId equals e.EventId
                                        where r.StudentId == studentId && e.StartDateTime > currentDate
                                        select new
                                        {
                                            e.EventId,
                                            e.Title,
                                            e.StartDateTime,
                                        }).ToListAsync();

            if (!upcomingEvents.Any())
                return Ok(new List<object>()); // return empty list instead of 404

            return Ok(upcomingEvents);
        }



        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
