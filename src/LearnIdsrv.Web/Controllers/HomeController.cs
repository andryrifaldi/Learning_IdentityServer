using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LearnIdsrv.Web.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace LearnIdsrv.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> CallAPI()
        //{
        //    var apiUrl = "https://localhost:44381/api/values";

        //    var accessToken = Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.GetTokenAsync(HttpContext, "access_token");
        //    var client = new HttpClient();

        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Result);

        //    HttpResponseMessage response = await client.GetAsync(apiUrl);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();
        //        ViewData["json"] = json;
        //    }
        //    else
        //    {
        //        ViewData["json"] = $"Error : {response.StatusCode}";
        //    }

        //    return View();
        //}

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://localhost:44381/api/values");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
