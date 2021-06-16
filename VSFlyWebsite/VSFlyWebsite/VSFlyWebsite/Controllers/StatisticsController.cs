using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VSFlyWebsite.Models;
using VSFlyWebsite.ViewModels.Statistics;

namespace VSFlyWebsite.Controllers
{
    public class StatisticsController : Controller
    {
        private static HttpClient _httpClient;

        static StatisticsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:44394/");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Flights(int flightNo)
        {
            TempData["noFlightOrSalePrice"] = null;

            try
            {
                string requestFlightUrl = "api/Flights/" + flightNo;

                string responseFlightBody = await _httpClient.GetStringAsync(requestFlightUrl);
                FlightModel flightModel = JsonConvert.DeserializeObject<FlightModel>(responseFlightBody);

                string requestSalePriceUrl = "api/Bookings/" + flightNo + "/SalePrice";
                string salePriceBody = await _httpClient.GetStringAsync(requestSalePriceUrl);

                double totalSalePrice = JsonConvert.DeserializeObject<double>(salePriceBody);

                StatisticsViewModel statisticsViewModel = new();

                statisticsViewModel.FlightModel = flightModel;
                statisticsViewModel.TotalFlightSalePrice = totalSalePrice;

                return View("index", statisticsViewModel);
            } catch (HttpRequestException e)
            {
                TempData["noFlightOrSalePrice"] = true;
                return View("index", null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Destination(string destination)
        {
            TempData["noDestination"] = null;
            TempData["noAveragePrice"] = null;

            List<FlightWithPassengerModel> flightWithPassengerModelList = null;

            try
            {
                string requestDestinationInfoUrl = "api/Bookings/Destination/" + destination;

                string responseDestinationInfoBody = await _httpClient.GetStringAsync(requestDestinationInfoUrl);
                flightWithPassengerModelList = JsonConvert.DeserializeObject<List<FlightWithPassengerModel>>(responseDestinationInfoBody);

                if (flightWithPassengerModelList.Count == 0)
                {
                    TempData["noDestination"] = true;
                    return View("index", null);
                }
            }
            catch (HttpRequestException e)
            {
                TempData["noDestination"] = true;
                return View("index", null);
            }

            double averageSalePrice = 0;

            try
            {
                string requestAverageSalePriceUrl = "api/Bookings/Destination/" + destination + "/AveragePrice";

                string responseAverageSalePriceBody = await _httpClient.GetStringAsync(requestAverageSalePriceUrl);
                averageSalePrice = JsonConvert.DeserializeObject<double>(responseAverageSalePriceBody);
            } catch (HttpRequestException e)
            {
                TempData["noAveragePrice"] = true;
            }

            StatisticsViewModel statisticsViewModel = new();

            statisticsViewModel.FlightWithPassengerModelList = flightWithPassengerModelList;
            statisticsViewModel.AverageDestinationSalePrice = averageSalePrice;

            return View("index", statisticsViewModel);
        }
    }
}
