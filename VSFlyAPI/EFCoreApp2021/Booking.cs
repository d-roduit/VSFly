using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreApp2021
{
    public class Booking
    {
        public int FlightNo { get; set; }
        public int PassengerID { get; set; }
        public double SalePrice { get; set; }
        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
    }
}
