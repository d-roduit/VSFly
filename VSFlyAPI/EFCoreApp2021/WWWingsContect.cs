using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreApp2021
{
    public class WWWingsContect : DbContext
    {
        public DbSet<Flight> FlightSet { get; set; }
        public DbSet<Pilot> PilotSet { get; set; }
        public DbSet<Passenger> PassengerSet { get; set; }
        public DbSet<Booking> BookingSet { get; set; }
        public static String ConnectionString { get; set; } =
            @"Server=(localDB)\MSSQLLocalDB;Database=WWWings_2021;Trusted_Connection=True;App=EFCoreApp2021;MultipleActiveResultSets=True";

        public WWWingsContect() { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // composed key
            builder.Entity<Booking>().HasKey(x => new { x.FlightNo, x.PassengerID });

            // mapping many to many relationship
            builder.Entity<Booking>()
                .HasOne(x => x.Flight)
                .WithMany(x => x.BookingSet)
                .HasForeignKey(x => x.FlightNo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(x => x.Passenger)
                .WithMany(x => x.BookingSet)
                .HasForeignKey(x => x.PassengerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}