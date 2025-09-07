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
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
                return BadRequest();

            // Check if the provided CollegeId exists
            var collegeExists = await _context.Colleges.AnyAsync(c => c.CollegeId == @event.CollegeId);
            if (!collegeExists)
                return BadRequest("College does not exist");

            // Check if the provided EventTypeId exists
            var eventTypeExists = await _context.EventTypes.AnyAsync(et => et.EventTypeId == @event.EventTypeId);
            if (!eventTypeExists)
                return BadRequest("EventType does not exist");

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // GET: api/Events/Popularity
        [HttpGet("Popularity")]
        public async Task<ActionResult<IEnumerable<object>>> GetEventPopularity()
        {
            var eventPopularity = await _context.Events
                .Select(e => new
                {
                    e.EventId,
                    e.Title,
                    RegistrationCount = _context.Registrations.Count(r => r.EventId == e.EventId)
                })
                .OrderByDescending(e => e.RegistrationCount)
                .ToListAsync();

            if (!eventPopularity.Any())
                return NotFound("No events found.");

            return Ok(eventPopularity);
        }


        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            // Check if the provided CollegeId exists
            var collegeExists = await _context.Colleges.AnyAsync(c => c.CollegeId == @event.CollegeId);
            if (!collegeExists)
                return BadRequest("College does not exist");

            // Check if the provided EventTypeId exists
            var eventTypeExists = await _context.EventTypes.AnyAsync(et => et.EventTypeId == @event.EventTypeId);
            if (!eventTypeExists)
                return BadRequest("EventType does not exist");

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // GET: api/Events/ByDate?date=2025-09-06
        [HttpGet("ByDate")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByDate([FromQuery] DateTime date)
        {
            var events = await _context.Events
                .Where(e => e.StartDateTime.Date == date.Date)
                .ToListAsync();

            if (!events.Any())
            {
                return NotFound("No events found on the given date.");
            }

            return events;
        }

        // GET: api/Events/Upcoming
        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<Event>>> GetUpcomingEvents()
        {
            var events = await _context.Events
                .Where(e => e.StartDateTime > DateTime.Now)
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();

            if (!events.Any())
            {
                return NotFound("No upcoming events found.");
            }

            return events;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventExists = await _context.Events.AnyAsync(e => e.EventId == id);
            if (!eventExists)
                return NotFound();

            // Raw SQL delete
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Events WHERE EventId = {0}", id);

            return NoContent();
        }

        // GET: api/Events/RegistrationCount/5
        [HttpGet("RegistrationCount/{eventId}")]
        public async Task<ActionResult<int>> GetRegistrationCount(int eventId)
        {
            var eventExists = await _context.Events.AnyAsync(e => e.EventId == eventId);
            if (!eventExists)
                return NotFound("Event does not exist.");

            // Using raw SQL to count registrations
            var count = await _context.Registrations
                .FromSqlRaw("SELECT * FROM Registrations WHERE EventId = {0}", eventId)
                .CountAsync();

            return Ok(count);
        }

        // GET: api/Events/AttendancePercentage/5
        [HttpGet("AttendancePercentage/{eventId}")]
        public async Task<ActionResult<double>> GetAttendancePercentage(int eventId)
        {
            // Check if event exists
            var eventExists = await _context.Events.AnyAsync(e => e.EventId == eventId);
            if (!eventExists)
                return NotFound("Event does not exist.");

            // Total registrations for this event
            var totalRegistrations = await _context.Registrations
                .Where(r => r.EventId == eventId)
                .CountAsync();

            if (totalRegistrations == 0)
                return Ok(0); // Avoid division by zero

            // Total attendances marked as present for this event
            var totalAttended = await (from a in _context.Attendances
                                       join r in _context.Registrations on a.RegistrationId equals r.RegistrationId
                                       where r.EventId == eventId && a.IsPresent
                                       select a).CountAsync();

            // Calculate percentage
            // Calculate percentage
            double percentage = Math.Round((totalAttended * 100.0) / totalRegistrations, 2);


            return Ok(percentage);
        }


        // GET: api/Events/ByType/3
        [HttpGet("ByType/{eventTypeId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByType(int eventTypeId)
        {
            // Check if EventType exists
            var eventTypeExists = await _context.EventTypes.AnyAsync(et => et.EventTypeId == eventTypeId);
            if (!eventTypeExists)
                return NotFound("EventType does not exist.");

            var events = await _context.Events
                .Where(e => e.EventTypeId == eventTypeId)
                .ToListAsync();

            if (!events.Any())
                return NotFound("No events found for the given EventType.");

            return Ok(events);
        }



        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
