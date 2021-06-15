using System;

namespace VSFlyWebsite.Models
{
    public class FlightModel
    {
        public int FlightNo { get; set; }
        public String Departure { get; set; }
        public String Destination { get; set; }
        public DateTime Date { get; set; }
        public double BasePrice { get; set; }
        public double SalePrice { get; set; }
    }
}
