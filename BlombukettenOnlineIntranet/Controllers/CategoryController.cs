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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            List<CategoryViewModel> listCategories = GetCategories();

            return View(listCategories);
        }

        private static List<CategoryViewModel> GetCategories()
        {
            List<CategoryViewModel> listCategories = new List<CategoryViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/getcategories").Result;

            if (response.IsSuccessStatusCode)
            {
                listCategories = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
            }

            return listCategories;
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<CategoryViewModel> listCategories = GetCategories();

            ViewBag.ParentId = new SelectList(listCategories, "Id", "Name", "ParentId");

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,ParentId")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                var param = Newtonsoft.Json.JsonConvert.SerializeObject(category);
                HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("api/intranet/create-category/", content).Result;

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/category/" + id).Result;

            var category= response.Content.ReadAsAsync<CategoryViewModel>().Result;

            List<CategoryViewModel> listCategories = GetCategories();

            ViewBag.ParentId = new SelectList(listCategories, "Id", "Name", "ParentId", category.ParentId);

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(int id, [Bind(Include = "Name,Price,ParentId")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                var paramtwo = Newtonsoft.Json.JsonConvert.SerializeObject(category);
                HttpContent contentTwo = new StringContent(paramtwo, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync("api/intranet/edit-category/" + id, contentTwo).Result;

                return RedirectToAction("Index");
                
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                HttpResponseMessage response = client.DeleteAsync("api/intranet/delete-category/" + id).Result;

                return RedirectToAction("Index");
            }
            return View();
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50716/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}