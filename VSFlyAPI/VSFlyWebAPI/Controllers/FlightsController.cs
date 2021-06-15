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

        // GET: api/Flights/TotalSalesForFlight/{id} return total purchase price of flight' bookings
        [HttpGet("{id}/SalePrice")]
        public async Task<ActionResult<double>> TotalSalesForFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id);
            double finalSale = 0.0;

            foreach (Booking booking in _context.BookingSet)
            {
                if(booking.FlightNo == id)
                finalSale += booking.SalePrice;
            }
            return finalSale;
        }

        // GET: api/Flights/Destination/{destination}
        // return average purchase price of destination's bookings
        [HttpGet]
        [Route("Destination/{destination}")]
        public async Task<ActionResult<double>> AverageFlightSaleForDestination(string destination)
        {
            var destinationPriceList = await _context.BookingSet.Where(booking => booking.Flight.Destination.Equals(destination))
                .Select(booking => booking.SalePrice).ToListAsync();

            double averageSale = destinationPriceList.Average();

            return Math.Round(averageSale, 2, MidpointRounding.ToEven);
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

        private double NumberOfPassengerOnPlane(Flight flight)
        {
           return _context.BookingSet.Where(booking => booking.FlightNo == flight.FlightNo).Count();
        }
    }
}
