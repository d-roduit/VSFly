using System.Collections.Generic;
using VSFlyWebAPI.Controllers.Models;

namespace VSFlyWebAPI.Models
{
    public class FlightWithPassengerModel
    {
        public FlightModel FlightModel { get; set; }
        public List<PassengerModel> PassengerModelList { get; set; }
    }
}
