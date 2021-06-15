using EFCoreApp2021;
using System.Linq;
using VSFlyWebAPI.Controllers.Models;

namespace VSFlyWebAPI.Controllers.extensions
{
    public static class ConverterExtensions
    {
        public static FlightModel ConvertToFlightModel(this Flight flight, WWWingsContect _context)
        {
            FlightModel flightModel = new();
            flightModel.Date = flight.Date;
            flightModel.Departure = flight.Departure;
            flightModel.Destination = flight.Destination;
            flightModel.FlightNo = flight.FlightNo;
            flightModel.BasePrice = flight.BasePrice;
            flightModel.SalePrice = 0;
            return flightModel;
        }

        public static Flight ConvertToFlight(this FlightModel flightModel)
        {
            Flight flight = new();
            flight.Date = flightModel.Date;
            flight.Departure = flightModel.Departure;
            flight.Destination = flightModel.Destination;
            flight.FlightNo = flightModel.FlightNo;
            flight.BasePrice = flightModel.BasePrice;
            return flight;
        }

        public static PassengerModel ConvertToPassengerModel(this Passenger passenger, Flight flight, WWWingsContect _context)
        {
            PassengerModel passengerModel = new();
            passengerModel.FirstName = passenger.FirstName;
            passengerModel.LastName = passenger.LastName;
            passengerModel.PurchasePrice = _context.BookingSet
                .Where(booking => booking.FlightNo == flight.FlightNo && booking.PassengerID == passenger.PersonID)
                .Select(booking => booking.SalePrice)
                .SingleOrDefault();
            return passengerModel;
        }

        public static Passenger ConvertToPassenger(this PassengerModel passengerModel)
        {
            Passenger passenger = new();
            passenger.FirstName = passengerModel.FirstName;
            passenger.LastName = passengerModel.LastName;
            
            return passenger;
        }
    }
}
