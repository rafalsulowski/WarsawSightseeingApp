using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

namespace ProjektZespolowy.Web.Controllers
{

    public class ForumController : Controller
    {

        private static readonly HttpClient client = new HttpClient();
        public List<PostDTO> posts;
        public ActionResult Index()
        {
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/GetAllPostsWithCommentsAndLikes");
            List<PostDTO>? posts = JsonSerializer.Deserialize<List<PostDTO>>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(posts);
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult CreatePost()
        {
            //Wylaczony do debugowania, potem wlaczyc
            /*if (HttpContext.Session.GetString("user-logged-in") != "true")
            {
                return RedirectToAction("Login", "Home");
            }*/
            return View();
        }
        [HttpPost]
        public ActionResult CreatePost(IFormCollection collection)
        {

            using (IEnumerator<KeyValuePair<string, StringValues>> e = collection.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    // do something with the item
                    Console.WriteLine(e.Current);
                }
            }
            Console.WriteLine(HttpContext.Session.GetString("token"));
            Console.WriteLine(collection["Title"]);
            Console.WriteLine(collection["Type"]);
            Console.WriteLine(collection["Content"]); 

            CreatePostDTO dto = new CreatePostDTO();
            dto.Title = collection["Title"];
            dto.Content = collection["Content"];

            if (collection["Type"] == "1")
            {
                dto.Type = PostType.Information;
            } else if (collection["Type"] == "2")
            {
                dto.Type = PostType.Discussion;
            } else if (collection["Type"] == "3")
            {
                Console.WriteLine("GLOSOWANIE spokok");
                dto.Type = PostType.Voting;
            }
            ;
            dto.VotesFor = 0; // PO CO??????
            dto.VotesAgainst = 0; // PO CO!?
            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PostAsJsonAsync<CreatePostDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"]+"api/Post/AddPost", dto);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return View();
        }
        [HttpPost]
        public ActionResult CreateComment(IFormCollection collection)
        {
            CreateCommentDTO dto = new CreateCommentDTO();
            dto.PostId = int.Parse(collection["PostId"]);
            dto.Content = collection["Comment"];

            String token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PostAsJsonAsync<CreateCommentDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/AddCommentToPost", dto);
            res.Wait();
            var result = res.Result;
            return Redirect("post/" + collection["PostId"]);
        }
        [HttpPost]
        public void Vote(IFormCollection collection)
        {
            /* Starałem się zrobić głosowanie w czystym html, nie działa idealnie
             * Wielokrotne zmienianie swojego głosu zapycha historię
             * Informacja kto głosuje do wyciągnięcia z danych sesji
             */
            using (IEnumerator<KeyValuePair<string, StringValues>> e = collection.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    Console.WriteLine(e.Current);
                }
            }
            Console.WriteLine("Vote recorded!");
        }

        public ActionResult Post(int id)
        {
            /*  Type = 0 informacja i komentarze wylaczone
             *  Type = 1 zwykla dyskusja
             *  Type = 2 voting, czyli dyskusja + sekcja glosowania
             *  
             *  Brak informacji o tym czy użytkowników ju kiedyś zagłosował, nie powinno być problemem funkcjonalnym tylko zmniejszeniem wygody
             * 
             
            com = new Comment() { Id = 1, Content = "fakty", UserId = 1, User = new User() { Username = "user_152325" }, PostId = id };
            coms.Add(com);

             */
            //Post post = new Post() { Id = id, Type = (PostType)2, VotesFor = 5, VotesAgainst = 3, Author = "user1", Title = "tytuł postu", UserId = 3, Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel facilisis volutpat est velit egestas dui id ornare arcu. Imperdiet proin fermentum leo vel. Egestas maecenas pharetra convallis posuere morbi. Amet justo donec enim diam. Lorem ipsum dolor sit amet consectetur. Amet dictum sit amet justo donec enim diam vulputate. Interdum consectetur libero id faucibus nisl tincidunt eget nullam non. Pharetra vel turpis nunc eget lorem dolor sed viverra ipsum. Sit amet nulla facilisi morbi tempus. Cras semper auctor neque vitae tempus quam pellentesque nec. Maecenas accumsan lacus vel facilisis volutpat. Nullam vehicula ipsum a arcu. Sem integer vitae justo eget magna fermentum iaculis eu. Morbi tristique senectus et netus et malesuada fames ac. Vitae nunc sed velit dignissim sodales. Elit duis tristique sollicitudin nibh sit amet commodo nulla. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Venenatis lectus magna fringilla urna porttitor rhoncus dolor purus. Egestas erat imperdiet sed euismod nisi.\r\n\r\nUt diam quam nulla porttitor massa. Nulla aliquet porttitor lacus luctus accumsan tortor posuere. Integer vitae justo eget magna fermentum iaculis. At risus viverra adipiscing at in. Sit amet est placerat in egestas erat. Ut diam quam nulla porttitor massa id neque. Eget mi proin sed libero enim. Nunc vel risus commodo viverra maecenas accumsan lacus vel. Pulvinar elementum integer enim neque. Non tellus orci ac auctor augue mauris augue neque gravida. Adipiscing bibendum est ultricies integer quis auctor elit sed vulputate. Neque vitae tempus quam pellentesque nec nam aliquam sem. Et netus et malesuada fames ac turpis egestas sed.\r\n\r\nNon diam phasellus vestibulum lorem sed risus ultricies. Donec ac odio tempor orci dapibus ultrices in iaculis. Eget nulla facilisi etiam dignissim diam quis enim. Egestas sed sed risus pretium quam vulputate dignissim suspendisse. Sagittis orci a scelerisque purus. Massa tempor nec feugiat nisl pretium. Et odio pellentesque diam volutpat commodo sed egestas egestas fringilla. Senectus et netus et malesuada fames ac turpis. Tincidunt arcu non sodales neque sodales ut etiam sit. Nunc congue nisi vitae suscipit tellus. Volutpat diam ut venenatis tellus in metus. Dui sapien eget mi proin sed. Vel pharetra vel turpis nunc eget lorem. Ut tristique et egestas quis ipsum suspendisse ultrices. Id nibh tortor id aliquet lectus proin nibh nisl condimentum. Nisi quis eleifend quam adipiscing. Risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est.\r\n\r\nRisus viverra adipiscing at in tellus integer. Tortor dignissim convallis aenean et tortor at risus. Sollicitudin ac orci phasellus egestas tellus. Aliquam etiam erat velit scelerisque in. Ut enim blandit volutpat maecenas. Malesuada proin libero nunc consequat interdum varius. Morbi leo urna molestie at elementum eu facilisis. Tincidunt tortor aliquam nulla facilisi. Pharetra convallis posuere morbi leo. Amet porttitor eget dolor morbi non arcu risus.\r\n\r\nPulvinar etiam non quam lacus suspendisse faucibus interdum posuere. Sollicitudin aliquam ultrices sagittis orci. Eget nunc lobortis mattis aliquam faucibus. Fermentum iaculis eu non diam phasellus vestibulum lorem. Vestibulum mattis ullamcorper velit sed. Odio ut sem nulla pharetra diam sit amet nisl. Purus ut faucibus pulvinar elementum integer. Posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus semper. Lectus vestibulum mattis ullamcorper velit sed ullamcorper morbi tincidunt. Ac ut consequat semper viverra nam libero justo laoreet.", Comments = coms };
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
               return RedirectToAction("Login", "Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"]+ "api/Post/GetPostWithCommentsAndLikes/" + id);
            PostDTO post = JsonSerializer.Deserialize<PostDTO>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            ViewData["user-id"] = HttpContext.Session.GetString("user-id");

            return View(post);
        }
        [HttpPost]
        public void LikePost(IFormCollection collection)
        {
            /* Starałem się zrobić głosowanie w czystym html, nie działa idealnie
             * Wielokrotne zmienianie swojego głosu zapycha historię
             * Informacja kto głosuje do wyciągnięcia z danych sesji
             */
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            CreateLikePostDTO dto = new CreateLikePostDTO();
            dto.PostId = int.Parse(collection["PostId"]);
            dto.IsLiked = int.Parse(collection["IsLiked"]) == 1 ? true : false;
            if(dto.IsLiked)
            {
                var res = client.DeleteAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/DeletePostLike?postId=" + dto.PostId);
                res.Wait();
            } else
            {
                var res = client.PostAsJsonAsync<CreateLikePostDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/LikePost", dto);
                res.Wait();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseString = client.GetStringAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/GetPostWithCommentsAndLikes/" + id);
            PostDTO trip = System.Text.Json.JsonSerializer.Deserialize<PostDTO>(responseString.Result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(trip);
        }

        [HttpPost]
        public IActionResult Edit(PostDTO dto)
        {
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.PutAsJsonAsync<PostDTO>(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/", dto);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return RedirectToAction("Post", "Forum", new { id = dto.Id });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            String token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = client.DeleteAsync(System.Configuration.ConfigurationManager.AppSettings["api_address"] + "api/Post/" + id);
            res.Wait();
            var result = res.Result;
            Console.WriteLine(result);
            return RedirectToAction("Index", "Forum");
        }
    }
    
}