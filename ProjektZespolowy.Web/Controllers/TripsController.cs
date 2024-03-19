using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NuGet.Protocol.Plugins;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text;
using NuGet.Common;

namespace ProjektZespolowy.Web.Controllers
{
    public class ActiveTripDTO
    {
        public int ViewType { get; set; }
        public int PlaceSeq { get; set; }
    }
    public class TripsController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        public List<Trip> trips;
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Users/" + userId + "/Trips");
            //var jsonData = (JObject)JsonConvert.DeserializeObject(responseString.Result);
            Console.WriteLine(responseString.Status);
            dynamic json = JObject.Parse(responseString.Result);
            //Console.WriteLine(json);//
            //Console.WriteLine(json["data"]);
            trips = System.Text.Json.JsonSerializer.Deserialize<List<Trip>>(json["data"].ToString(), new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            });
            
            
            return View(trips);
        }
        public IActionResult AddPlace()
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Places");
            List<Place> places = System.Text.Json.JsonSerializer.Deserialize<List<Place>>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (HttpContext.Session.GetString("edited-trip") == null)
            {
                return RedirectToAction("Index", "Trips");
            }
            ViewData["edited-trip-day"] = HttpContext.Session.GetString("edited-trip-day");
            ViewData["edited-trip-estimated-cost"] = HttpContext.Session.GetString("edited-trip-estimated-cost");
            ViewData["edited-trip-planned-cost"] = HttpContext.Session.GetString("edited-trip-planned-cost");
            return View(places);
        }
        [HttpPost]
        public IActionResult AddPlace([FromForm]AddPlaceToTripDTO dto)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (HttpContext.Session.GetString("edited-trip") == null){
                return RedirectToAction("Index", "Trips");
            }
            dto.TripId = int.Parse(HttpContext.Session.GetString("edited-trip"));
            dto.Sequence = int.Parse(HttpContext.Session.GetString("edited-trip-sequence-last"));
            Type t = dto.GetType(); // Where obj is object whose properties you need.
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                System.Console.WriteLine(p.Name + " : " + p.GetValue(dto));
            }
            //
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PostAsJsonAsync<AddPlaceToTripDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/AddPlaceToTrip", dto);
            res.Wait();
            Console.WriteLine(res.Result);
            var url = "/Trips/Details/" + int.Parse(HttpContext.Session.GetString("edited-trip"));
            Console.WriteLine(url);
            return Redirect(url);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTripDTO dto)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PostAsJsonAsync<CreateTripDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/AddTrip", dto);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return RedirectToAction("Index","Trips");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpContext.Session.SetString("edited-trip", id.ToString());
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/GetTripWithPlaces/" + id);
            TripDTO trip = System.Text.Json.JsonSerializer.Deserialize<TripDTO>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (trip.IsPublic == false) {
                if (trip.UserId != int.Parse(HttpContext.Session.GetString("user-id").ToString()))
                {
                    return RedirectToAction("Index", "Trips");
                }
            }
            HttpContext.Session.SetString("edited-trip-sequence-last", (trip.Places.Count()+1).ToString());
            HttpContext.Session.SetString("edited-trip-estimated-cost", (trip.Places.Sum(item=>item.EstimatedCost)).ToString());
            HttpContext.Session.SetString("edited-trip-planned-cost", (trip.Budget.ToString()));
            HttpContext.Session.SetString("edited-trip-day", ((int)trip.Date.DayOfWeek).ToString());
            return View(trip);
        }
        [HttpGet]
        public IActionResult ActiveTrip()
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            /*ActiveTrip to strona w stylu SPA - wszystko dzieje się w JS
             Wszystkie żądania POST służa do zapisywania aktualnego postępu w danych sesji
            Dzięki temu w teorii można wyjść z przeglądarki, a następnie wrócić do aktualnego przystanku bez problemu*/
            string id = HttpContext.Session.GetString("edited-trip");
            if (HttpContext.Session.GetString("edited-trip") == null)
            {
                //W produkcji zamienic na przekierowanie do loginu/trips
                Console.WriteLine("trips controller warning");
                id = "1";
            }
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/GetTripWithPlaces/" + id);
            TripDTO trip = System.Text.Json.JsonSerializer.Deserialize<TripDTO>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            string ViewType = HttpContext.Session.GetString("activetrip-viewtype");
            string PlaceSeq = HttpContext.Session.GetString("activetrip-placeseq");
            if (ViewType == null || PlaceSeq == null){
                ViewType = "0";
                PlaceSeq = "-1";
            }
            ViewData["ViewType"] = ViewType;
            ViewData["PlaceSeq"] = PlaceSeq;
            return View(trip);
        }
        
        [HttpPost]
        public IActionResult ActiveTrip([FromBody] ActiveTripDTO dto)
        {
            HttpContext.Session.SetString("activetrip-viewtype", dto.ViewType.ToString());
            HttpContext.Session.SetString("activetrip-placeseq", dto.PlaceSeq.ToString());
            return new EmptyResult();

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/GetTripWithPlaces/" + id);
            TripDTO trip = System.Text.Json.JsonSerializer.Deserialize<TripDTO>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(trip);
        }

        [HttpPost]
        public IActionResult Edit(TripDTO dto)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Console.WriteLine(dto);
            //TripDTO dto = new TripDTO();
            Type t = dto.GetType(); // Where obj is object whose properties you need.
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                System.Console.WriteLine(p.Name + " : " + p.GetValue(dto));
            }
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PutAsJsonAsync<TripDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/", dto);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return RedirectToAction("Details", "Trips", new { id = dto.Id});
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetString("user-id");
            if (userId == "" || userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.DeleteAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Trip/" + id);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return RedirectToAction("Index", "Trips");
        }
    }
}
