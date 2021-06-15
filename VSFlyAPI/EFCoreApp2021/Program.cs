using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCoreApp2021
{
    class Program
    {
        static void Main(string[] args)
        {
            #region DB Creation
            // Database Creation

            var ctx = new WWWingsContect();
            var e = ctx.Database.EnsureCreated();

            if (e)
            {
                Console.WriteLine("Database has been created");
            }
            else
            {
                Console.WriteLine("Database already exists");
            }
            Console.WriteLine("done.");
            #endregion

            #region Pilots
            //Creating pilot objects

            Pilot p = new Pilot { FirstName = "Emil", LastName = "Lang", Salray = 9500 };
            ctx.PilotSet.Add(p);
            Pilot p1 = new Pilot { FirstName = "Charles", LastName = "Doe", Salray = 7000 };
            ctx.PilotSet.Add(p1);
            Pilot p2 = new Pilot { FirstName = "Jessica", LastName = "Rise", Salray = 8250 };
            ctx.PilotSet.Add(p2);

            ctx.SaveChanges();
            #endregion

            #region Flights
            //Creating flight objects
            var newFlight = new Flight
            {
                Departure = "Geneva",
                Destination = "Paris",
                Date = DateTime.Parse("20/07/2021 08:35"),
                Seats = 10,
                BasePrice = 150,
                PilotId = p.PersonID
            };
            ctx.Add(newFlight);
            ctx.SaveChanges();
            var newFlight2 = new Flight
            {
                Departure = "Paris",
                Destination = "Zurich",
                Date = DateTime.Parse("22/07/2021 17:40"),
                Seats = 10,
                BasePrice = 180,
                PilotId = p.PersonID
            };
            ctx.Add(newFlight2);
            ctx.SaveChanges();
            var newFlight3 = new Flight
            {
                Departure = "London",
                Destination = "Basel",
                Date = DateTime.Parse("23/07/2021 18:40"),
                Seats = 10,
                BasePrice = 90,
                PilotId = p1.PersonID
            };
            ctx.Add(newFlight3);
            ctx.SaveChanges();
            var newFlight4 = new Flight
            {
                Departure = "Madrid",
                Destination = "Geneva",
                Date = DateTime.Parse("28/07/2021 09:55"),
                Seats = 10,
                BasePrice = 75,
                PilotId = p2.PersonID
            };
            ctx.Add(newFlight4);
            ctx.SaveChanges();
            var newFlight5 = new Flight
            {
                Departure = "Geneva",
                Destination = "Paris",
                Date = DateTime.Parse("22/02/2021 10:40"),
                Seats = 10,
                BasePrice = 200,
                PilotId = p2.PersonID
            };
            ctx.Add(newFlight5);
            ctx.SaveChanges();
            #endregion

            #region Test Passengers
            // Creating passenger objects
            Passenger passengerTest1 = new Passenger { FirstName = "Wess", LastName = "Gibbins" };
            Passenger passengerTest2 = new Passenger { FirstName = "Michaela", LastName = "Pratt" };
            Passenger passengerTest3 = new Passenger { FirstName = "Laurel", LastName = "Castillo" };

            ctx.PassengerSet.Add(passengerTest1);
            ctx.PassengerSet.Add(passengerTest2);
            ctx.PassengerSet.Add(passengerTest3);
            ctx.SaveChanges();
            #endregion

            #region Bookings

            ctx.BookingSet.Add(new Booking { Flight = ctx.FlightSet.Find(1), PassengerID = 1, SalePrice = 60});
            ctx.BookingSet.Add(new Booking { Flight = ctx.FlightSet.Find(1), PassengerID = 2, SalePrice = 140 });
            ctx.BookingSet.Add(new Booking { Flight = ctx.FlightSet.Find(2), PassengerID = 3, SalePrice = 100});
            ctx.SaveChanges();

            #endregion

            #region Selection of all flights
            Console.WriteLine();
            Console.WriteLine("Selection and display of all flights");

            // Selection of the whole list 

            foreach (Flight flight in ctx.FlightSet)
            {
                Console.WriteLine("Date: {0} Departure: {1} Destination: {2} Seats: {3} Price: {4}", flight.Date, flight.Departure, flight.Destination, flight.Seats, flight.BasePrice);
            }
            #endregion

            #region Selection of specific flight from FlightSet
            //Selection of records base en criterias
            Console.WriteLine();
            Console.WriteLine("Selection of criterias with from");

            var queryCDGDeparture = from flight in ctx.FlightSet
                                        // where flight.Departure == "GVA"
                                    where flight.Seats > 125 && flight.Destination.Contains("BSL")
                                    select flight;

            foreach (Flight flight in queryCDGDeparture)
            {
                Console.WriteLine("Date: {0} Departure: {1} Destination: {2} Seats: {3}", flight.Date, flight.Departure, flight.Destination, flight.Seats);
            }
            #endregion

            #region Selection of specific flight with Lambda expression
            //Selection of records base en criterias
            Console.WriteLine();
            Console.WriteLine("Selection of criterias with lambda");

            var lamb = ctx.FlightSet.Where(f => f.Seats < 175 && f.Departure.StartsWith("G"));

            foreach (Flight flight in lamb)
            {
                Console.WriteLine("Date: {0} Departure: {1} Destination: {2} Seats: {3} Price {4}", flight.Date, flight.Departure, flight.Destination, flight.Seats, flight.BasePrice);
            }
            #endregion

            #region Specific flight update 

            //Update a flight
            Console.WriteLine();
            Console.WriteLine("update a flight");

            var updateFlight = ctx.FlightSet.Find(1);

            if (updateFlight != null)
            {
                updateFlight.Seats = 1;
            }
            ctx.SaveChanges();

            foreach (Flight flight in lamb)
            {
                Console.WriteLine("Date: {0} Departure: {1} Destination: {2} Seats: {3}", flight.Date, flight.Departure, flight.Destination, flight.Seats);
            }
            #endregion

            #region Removal of flights above flightId 8

            //remove all records above a certain amount from the database
            Console.WriteLine();
            Console.WriteLine("remove all flights above ");

            var removeFlight = from flight in ctx.FlightSet where flight.FlightNo > 8 select flight;

            //You can delete a range of flights
            ctx.RemoveRange(removeFlight);

            // You can delete flights from a flightset
            // foreach(Flight flight in removeFlight) {
            //    ctx.Remove(flight);
            // }
            // ctx.SaveChanges();

            //You can delete without callback directly with SQL request
            //ctx.Database.ExecuteSqlRaw("DELETE FROM FLIGHTSET WHERE FlightNo > 3");

            foreach (Flight flight in lamb)
            {
                Console.WriteLine("Date: {0} Departure: {1} Destination: {2} Seats: {3}", flight.Date, flight.Departure, flight.Destination, flight.Seats);
            }
            #endregion

            //Select Flights and pilots and display them 
            Console.WriteLine();
            Console.WriteLine("Display Flights and pilots");

            var q3 = from f in ctx.FlightSet select f;

            foreach (Flight flight in q3)
            {
                ctx.Entry(flight).Reference(x => x.Pilot).Load();

                Console.WriteLine("Date: {0} Departure: {1} PilotName: {2} PilotSalary: {3}",
                       flight.Date, flight.Departure, flight.Pilot.FirstName, flight.Pilot.Salray);
            }

            //Select Flights and pilots and display them with join
            Console.WriteLine();
            Console.WriteLine("Display Flights and pilots with a JOIN");

            var q4 = from f in ctx.FlightSet.Include(x => x.Pilot)
                     select f;

            foreach (Flight flight in q4)
            {
                Console.WriteLine("Date: {0} Departure: {1} PilotName: {2} PilotSalary: {3}",
                      flight.Date, flight.Departure, flight.Pilot.FirstName, flight.Pilot.Salray);
            }

            //Start query with pilots and not flights
            Console.WriteLine();
            Console.WriteLine("Query with pilots");

            var q5 = from pilot in ctx.PilotSet.Include(x => x.FlightasPilotSet) select pilot;

            foreach (Pilot pilot in q5)
            {
                //ctx.Entry(pilot).Collection(x => x.FlightasPilotSet).Load();

                Console.WriteLine("PilotName: {0} Salary: {1} Flight Hours: {2} List of flights: {3}",
                       pilot.FirstName, pilot.Salray, pilot.FlightHours, pilot.FlightasPilotSet.Count());

                foreach (Flight flight in pilot.FlightasPilotSet)
                {
                    Console.WriteLine("- Date: {0} Departure: {1} Seats: {2}", flight.Date, flight.Destination, flight.Seats);
                }
            }

            //Query Relation many to many with passengers and flights
            Console.WriteLine();
            Console.WriteLine("Query Relation many to many with passengers and flights");
            //Add passengers
            Console.WriteLine("adding passengers to the flight");

            //Passenger pass = new Passenger { GivenName = "George", weight = 77 };

            //ctx.PassengerSet.Add(pass);
            //ctx.SaveChanges();

            //Add bookings to book flights
            Console.WriteLine();
            Console.WriteLine("add bookings, to book flights");

            //ctx.BookingSet.Add(new Booking { Flight = ctx.FlightSet.Find(1), PassengerID = 1 });
            //ctx.BookingSet.Add(new Booking { Flight = ctx.FlightSet.Find(1), PassengerID = 2 });
            //ctx.SaveChanges();

            //select booked flights, passengers and their booking, flights and their passengers
            Console.WriteLine();
            Console.WriteLine("add bookings, to book flights");

            var q = from b in ctx.BookingSet.Include("Flight").Include("Passenger")
                    select b;

            foreach (Booking b in q)
            {
                Console.WriteLine("{0} {1} {2} {3} {4}", 
                                                b.Flight.Date,
                                                b.Flight.Departure,
                                                b.Flight.Destination,
                                                b.Passenger.FirstName,
                                                b.Passenger.LastName);
            }

            Console.WriteLine();
            Console.WriteLine("add bookings, to book flights");

            var q6 = from passenger in ctx.PassengerSet.Include(x => x.BookingSet).ThenInclude(x => x.Flight).ThenInclude(x => x.Pilot)
                    select passenger;

            foreach (Passenger pass in q6)
            {
                Console.WriteLine("{0} {1} {2}", pass.PersonID, pass.LastName, pass.BookingSet.Count());

                foreach (Booking book in pass.BookingSet)
                {
                    Console.WriteLine("{0} {1} {2}", book.Flight.Date, book.Flight.Departure, book.Flight.Pilot.FirstName);
                }
            }

            Console.ReadKey();
        }
    }
}
