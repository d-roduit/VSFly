﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebsite.Models
{
    public class FlightWithPassengerModel
    {
        public FlightModel FlightModel { get; set; }
        public List<PassengerModel> PassengerModelList { get; set; }
    }
}
