using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models;
using System.Text.Json;

namespace ProjektZespolowy.Web.Controllers
{
    public class MapController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        public List<Place> places;
        public IActionResult Index()
        {
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Places");
            List<Place> places = JsonSerializer.Deserialize<List<Place>>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(places);
        }
    }
}