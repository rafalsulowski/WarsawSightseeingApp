using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Web.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;
using ProjektZespolowy.Models.DTO;
using ProjektZespolowy.Models;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Reflection;
using NuGet.Common;
using Newtonsoft.Json.Linq;

namespace ProjektZespolowy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient client = new HttpClient();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            /* tu chodzi o to zeby nie cofac do landing page*/
            string login = HttpContext.Session.GetString("user-logged-in");
            if (login == "true")
            {
                return RedirectToAction("Landing", "Home");
            }
            else
            {
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("token", "");
            HttpContext.Session.SetString("user-logged-in", "false");
            HttpContext.Session.SetString("user-id", "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public IActionResult Login(LoginDTO login)
        {
            var res = client.PostAsJsonAsync<LoginDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/users/login", login);
            res.Wait();
            var result = res.Result;
            var token = result.Content.ReadAsStringAsync();
            if (result.StatusCode.ToString() != "OK")
            {
                Response.StatusCode = 403;
                return Content("Error - bad credentials");
            }
            HttpContext.Session.SetString("token", token.Result);
            HttpContext.Session.SetString("user-logged-in", "true");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Users/UserDataFromToken");
            User user = System.Text.Json.JsonSerializer.Deserialize<User>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            HttpContext.Session.SetString("user-id", user.Id.ToString());
            return RedirectToAction("Landing", "Home");
            //return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterUserDTO dto)
        {
            var res = client.PostAsJsonAsync<RegisterUserDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"]+"api/users/", dto);
            res.Wait();
            var result = res.Result;
            var token = result.Content.ReadAsStringAsync();
            if (result.StatusCode.ToString() != "OK")
            {
                Response.StatusCode = 403;
                return Content("Error - bad credentials");
            }
            return RedirectToAction("Login", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Landing()
        {
            return View();
        }

        public IActionResult Profile()
        {
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Users/UserDataFromToken");
            User user = System.Text.Json.JsonSerializer.Deserialize<User>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            ViewData["username"] = user.Username;

            string endpoint = "api/Users/" + HttpContext.Session.GetString("user-id") + "/Posts";
            Console.WriteLine(endpoint);
            responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + endpoint);
            //Console.WriteLine(responseString.Status);
            dynamic json = JObject.Parse(responseString.Result);
            List<Post>? posts = System.Text.Json.JsonSerializer.Deserialize<List<Post>>(json["data"].ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(posts);
        }
    }
}