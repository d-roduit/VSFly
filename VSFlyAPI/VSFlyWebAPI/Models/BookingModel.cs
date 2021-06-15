using System.Collections.Generic;
using VSFlyWebAPI.Controllers.Models;

namespace VSFlyWebAPI.Models
{
    public class BookingModel
    {
        public FlightModel FlightModel { get; set; }
        public List<PassengerModel> PassengerModelList { get; set; }
    }
}
