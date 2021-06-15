using EFCoreApp2021;
using System;
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

            /*   1.	If the airplane is more than 80% full regardless of the date:
                    a. sale price = 150% of the base price
                 2.	If the plane is filled less than 20% less than 2 months before departure:
                    a. sale price = 80% of the base price
                 3.	If the plane is filled less than 50% less than 1 month before departure:
                    a. sale price = 70% of the base price
                 4.	4. In all other cases:
                    a. sale price = base price
            */

            double seatsBooked = _context.BookingSet.Where(booking => booking.FlightNo == flight.FlightNo).Count();
            double planeCapacity = (double)flight.Seats;
            double flightFullness = (seatsBooked / planeCapacity )* 100;
            double modifier = 1;

            int delta = (flight.Date - DateTime.Now).Days;
                
            if (flightFullness < 20 && delta < 60 ) 
            {
                modifier = 0.7;
            }
                
            if (flightFullness < 50 && delta < 30 )
            {
                modifier = 0.8;
            }

            if (flightFullness > 80)
            {
                modifier = 1.5;
            }
         
            flightModel.SalePrice = Math.Round(flight.BasePrice * modifier, 2);
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
