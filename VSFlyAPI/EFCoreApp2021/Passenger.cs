using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreApp2021
{
    public class Passenger : Person
    {
        public virtual ICollection<Booking> BookingSet { get; set; }
    }
}
