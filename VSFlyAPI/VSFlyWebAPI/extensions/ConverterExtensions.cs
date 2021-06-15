using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyWebAPI.Controllers.extensions
{
    public static class ConverterExtensions
    {
        public static Models.FlightModel ConverterToFlightM(this EFCoreApp2021.Flight f)
        {
            Models.FlightModel fm = new Models.FlightModel();
            fm.Date = f.Date;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.FlightNo = f.FlightNo;
            fm.BasePrice = f.BasePrice;
            return fm;
        }

        public static EFCoreApp2021.Flight ConvertToFlightEF(this Models.FlightModel f)
        {
            EFCoreApp2021.Flight fm = new EFCoreApp2021.Flight();
            fm.Date = f.Date;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.FlightNo = f.FlightNo;
            fm.BasePrice = f.BasePrice;
            return fm;
        }
    }
}
