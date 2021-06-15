using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreApp2021;
using VSFlyWebAPI.Controllers.Models;
using VSFlyWebAPI.Models;
using VSFlyWebAPI.Controllers.extensions;

namespace VSFlyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly WWWingsContect _context;

        public BookingsController(WWWingsContect context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingSet()
        {
            return await _context.BookingSet.ToListAsync();
        }

        


        // GET: api/Bookings
        [HttpGet]
        [Route("{destination}")]
        public async Task<ActionResult<IEnumerable<FlightWithPassengerModel>>> GetAllTicketSoldForDestination(string destination)
        {
            var destinationFlightList = await _context.FlightSet.Where(flight => flight.Destination.Equals(destination)).ToListAsync();
            var destinationBookingList = await _context.BookingSet.Where(booking => booking.Flight.Destination.Equals(destination)).ToListAsync();

            List<FlightWithPassengerModel> flightWithPassengerModelList = new();

            foreach (Flight flight in destinationFlightList)
            {
                FlightWithPassengerModel flightWithPassengerModel = new();
                flightWithPassengerModel.FlightModel = flight.ConvertToFlightModel(_context);
                flightWithPassengerModel.PassengerModelList = new List<PassengerModel>();
                foreach (Booking booking in destinationBookingList)
                {
                    if (flight.FlightNo == booking.FlightNo)
                    {

                        Passenger passenger = _context.PassengerSet.Find(booking.PassengerID);

                        PassengerModel passengerModel = new();
                        passengerModel.FlightNo = flight.FlightNo;
                        passengerModel.FirstName = passenger.FirstName;
                        passengerModel.LastName = passenger.LastName;
                        passengerModel.PurchasePrice = booking.SalePrice;
                        flightWithPassengerModel.PassengerModelList.Add(passengerModel);
                    }
                    
                }
                flightWithPassengerModelList.Add(flightWithPassengerModel);
            }
            return flightWithPassengerModelList;
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PassengerModel>> PostBooking(PassengerModel passengerModel)
        {
            if (passengerModel == null)
            {
                return NotFound();
            }

            var passenger = passengerModel.ConvertToPassenger();
            _context.PassengerSet.Add(passenger);

            await _context.SaveChangesAsync();
           
            int passengerIndex = await _context.PassengerSet.Where(passenger => passenger.FirstName.Equals(passengerModel.FirstName) &&
                passenger.LastName.Equals(passengerModel.LastName))
                .Select(passenger => passenger.PersonID).FirstAsync();

            _context.BookingSet.Add(new Booking { FlightNo = passengerModel.FlightNo, PassengerID = passengerIndex, 
                SalePrice =  passengerModel.PurchasePrice });
            await _context.SaveChangesAsync();

            return StatusCode(200);

        }

        private bool BookingExists(int id)
        {
            return _context.BookingSet.Any(e => e.FlightNo == id);
        }
    }
}
