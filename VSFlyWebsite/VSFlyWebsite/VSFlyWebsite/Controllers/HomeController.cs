using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VSFlyWebsite.Models;

namespace VSFlyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient _httpClient;

        static HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:44394/");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<FlightModel> flightModelList = null;

            try
            {
                string requestUrl = "api/Flights/Available";
                string responseBody = await _httpClient.GetStringAsync(requestUrl);
                flightModelList = JsonConvert.DeserializeObject<List<FlightModel>>(responseBody);
            }
            catch (HttpRequestException e)
            {
                return RedirectToAction("Error");
            }

            return View(flightModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PassengerModel passengerModel)
        {
            string requestUrl = "api/Bookings";
            string jsonDataString = JsonConvert.SerializeObject(passengerModel);
            HttpContent jsonContent = new StringContent(jsonDataString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, jsonContent);

            bool isPostSuccess = false;

            if (response.IsSuccessStatusCode)
            {
                isPostSuccess = true;
            }

            TempData["isPostSuccess"] = isPostSuccess;

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
