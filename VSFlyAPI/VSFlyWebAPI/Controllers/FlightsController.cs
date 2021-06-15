using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreApp2021;
using VSFlyWebAPI.Controllers.Models;
using VSFlyWebAPI.Controllers.extensions;

namespace VSFlyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly WWWingsContect _context;

        public FlightsController(WWWingsContect context)
        {
            _context = context;
        }

        // GET: api/Flights/Available
        [HttpGet]
        [Route("Available")]
        public async Task<ActionResult<IEnumerable<FlightModel>>> GetAvailableFlights()
        {
            var flightList = await _context.FlightSet.Where(flight => flight.Date >= DateTime.Now).ToListAsync();
            List<FlightModel> flightModelList = new List<FlightModel>();

            foreach (Flight flight in flightList)
            {
                if (!IsFlightFull(flight))
                {
                    flightModelList.Add(flight.ConvertToFlightModel(_context));
                }
            }

            return flightModelList;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightNo)
            {
                return BadRequest();
            }

            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
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

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(FlightModel flight)
        {
            _context.FlightSet.Add(flight.ConvertToFlight());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FlightNo }, flight);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.FlightSet.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightExists(int flightNo)
        {
            return _context.FlightSet.Any(flight => flight.FlightNo == flightNo);
        }

        private bool IsFlightFull(Flight flight)
        {
            if (!FlightExists(flight.FlightNo))
            {
                throw new ArgumentException("No flight exists for flightNo " + flight.FlightNo);
            }

            int seatsBooked = _context.BookingSet.Where(booking => booking.FlightNo == flight.FlightNo).Count();

            return seatsBooked >= flight.Seats;
        }
    }
}
