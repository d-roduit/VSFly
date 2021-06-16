using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSFlyWebsite.Models;

namespace VSFlyWebsite.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public FlightModel FlightModel { get; set; }
        public double TotalFlightSalePrice { get; set; }
        public List<FlightWithPassengerModel> FlightWithPassengerModelList { get; set; }
        public double AverageDestinationSalePrice { get; set; }
    }
}
