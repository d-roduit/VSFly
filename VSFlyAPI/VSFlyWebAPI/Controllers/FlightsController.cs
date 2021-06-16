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
        public async Task<ActionResult<FlightModel>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight.ConvertToFlightModel(_context);
        }

        // GET: api/Flights/{id}/SalePrice
        // Return the SalePrice for a specific flight
        [HttpGet("{id}/SalePrice")]
        public async Task<ActionResult<double>> SalePriceForFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id);

            return flight.ConvertToFlightModel(_context).SalePrice;
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
