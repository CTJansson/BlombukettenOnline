using BlombukettenOnlineIntranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BlombukettenOnlineIntranet.Controllers
{
    public class ColorController : Controller
    {
        public ActionResult Index()
        {
            List<ColorViewModel> listColors = new List<ColorViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/getcolors").Result;

            if (response.IsSuccessStatusCode)
            {
                listColors = response.Content.ReadAsAsync<List<ColorViewModel>>().Result;
            }
            return View(listColors);
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50716/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ColorViewModel color)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                var param = Newtonsoft.Json.JsonConvert.SerializeObject(color);
                HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("api/intranet/create-color/", content).Result;

                return RedirectToAction("Index");

            }
            return View();
        } 

        [HttpGet]
        public ActionResult Edit(int id)
        {
            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/color/" + id).Result;

            var color = response.Content.ReadAsAsync<ColorViewModel>().Result;

            return View(color);
        }

        [HttpPost]
        public ActionResult Edit(int id,ColorViewModel color)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                var paramtwo = Newtonsoft.Json.JsonConvert.SerializeObject(color);
                HttpContent contentTwo = new StringContent(paramtwo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync("api/intranet/edit-color/" + id, contentTwo).Result;

                return RedirectToAction("Index"); 
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                HttpResponseMessage response = client.DeleteAsync("api/intranet/delete-color/" + id).Result;

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}