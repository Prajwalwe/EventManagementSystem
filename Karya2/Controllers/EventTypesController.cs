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
    public class EventTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EventTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventType>>> GetEventTypes()
        {
            return await _context.EventTypes.ToListAsync();
        }

        // GET: api/EventTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventType>> GetEventType(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);

            if (eventType == null)
            {
                return NotFound();
            }

            return eventType;
        }

        // PUT: api/EventTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventType(int id, EventType eventType)
        {
            if (id != eventType.EventTypeId)
            {
                return BadRequest();
            }

            _context.Entry(eventType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventTypeExists(id))
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

        // POST: api/EventTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventType>> PostEventType(EventType eventType)
        {
            _context.EventTypes.Add(eventType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventType", new { id = eventType.EventTypeId }, eventType);
        }

        // DELETE: api/EventTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventType(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);
            if (eventType == null)
            {
                return NotFound();
            }

            _context.EventTypes.Remove(eventType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventTypeExists(int id)
        {
            return _context.EventTypes.Any(e => e.EventTypeId == id);
        }
    }
}
