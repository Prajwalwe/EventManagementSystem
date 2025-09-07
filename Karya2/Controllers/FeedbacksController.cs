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
    public class FeedbacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            if (id != feedback.FeedbackId)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedback", new { id = feedback.FeedbackId }, feedback);
        }

        // GET: api/Feedbacks/Event/5/AverageRating
        [HttpGet("Event/{eventId}/AverageRating")]
        public async Task<ActionResult<double>> GetAverageRatingForEvent(int eventId)
        {
            // Step 1: Get RegistrationIds for the event
            var registrationIds = await _context.Registrations
                .Where(r => r.EventId == eventId)
                .Select(r => r.RegistrationId)
                .ToListAsync();

            if (!registrationIds.Any())
            {
                return NotFound("No registrations found for this event.");
            }

            // Step 2: Get AttendanceIds for present students
            var attendanceIds = await _context.Attendances
                .Where(a => registrationIds.Contains(a.RegistrationId) && a.IsPresent)
                .Select(a => a.AttendanceId)
                .ToListAsync();

            if (!attendanceIds.Any())
            {
                return NotFound("No attendees marked as present for this event.");
            }

            // Step 3: Get ratings for those AttendanceIds
            var ratings = await _context.Feedbacks
                .Where(f => attendanceIds.Contains(f.AttendanceId))
                .Select(f => f.Rating)
                .ToListAsync();

            if (!ratings.Any())
            {
                return NotFound("No feedback found for this event.");
            }

            // Step 4: Return average rating
            double averageRating = ratings.Average();
            return Ok(averageRating);
        }


        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
