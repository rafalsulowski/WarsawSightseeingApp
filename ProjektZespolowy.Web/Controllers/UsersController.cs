using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjektZespolowy.Models;

namespace ProjektZespolowy.Web.Controllers
{
    public class UsersController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        // GET: HomeController1
        public ActionResult Index()
        {
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Users/");
            //get users here
            List<User>? users = JsonSerializer.Deserialize<List<User>>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(users);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Users/" + id);
            try
            {
                User? user = JsonSerializer.Deserialize<User>(responseString.Result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(user);
            }
            catch (Exception e)
            {
                //DANGEROUS
                Console.WriteLine(responseString.Result);
                return View();
            }
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}