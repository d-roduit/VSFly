using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreApp2021
{
    public class Pilot:Employee
    {
        public int? FlightHours { get; set; }
        public virtual ICollection<Flight> FlightasPilotSet { get; set; }
    }
}
