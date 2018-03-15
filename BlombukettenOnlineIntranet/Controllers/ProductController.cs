using BlombukettenOnlineIntranet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace BlombukettenOnlineIntranet.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            
            List<ProductViewModel> listProducts = new List<ProductViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase").Result;

            if (response.IsSuccessStatusCode)
            {
                listProducts = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            }
            return View(listProducts);
        }

        [HttpGet]
        public ActionResult AllProducts()
        {
            List<ProductViewModel> listProducts = new List<ProductViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase").Result;


            if (response.IsSuccessStatusCode)
            {
                listProducts = response.Content.ReadAsAsync<List<ProductViewModel>>().Result;
            }

            return View(listProducts);
        }
        [HttpGet]
        public ActionResult Create()
        {
            List<ColorViewModel> listColors = new List<ColorViewModel>();
            List<CategoryViewModel> listCategories = new List<CategoryViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/getcolors").Result;
            HttpResponseMessage responseCat = client.GetAsync("api/datebase/getcategories").Result;

            if (response.IsSuccessStatusCode)
            {
                listColors = response.Content.ReadAsAsync<List<ColorViewModel>>().Result;
                listCategories = responseCat.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
            }

            ViewBag.Color = new SelectList(listColors, "Id", "Name");
            ViewBag.Category = new SelectList(listCategories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Description,ColorId,CategoryId,Price, Image")]  CreateProductModel model, HttpPostedFileBase file )
        {
            string imagePath = Directory.GetParent(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath).Parent.FullName + @"\BlombukettenOnline\Content\images";

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(imagePath, fileName);
                file.SaveAs(path);

                var pathTwo = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(pathTwo);

                model.Image = fileName;
            }

            if (model.Image == null)
            {
                model.Image = "BildSaknas.jpg";
            }

            if (ModelState.IsValid)
            {
                Post(model);

                return RedirectToAction("Index");         
      
            }
            Create();

            return View();
        }

        public ActionResult Details(int id)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = CreateHttpClient();

                HttpResponseMessage response = client.GetAsync("api/datebase/" + id).Result;

                var product = response.Content.ReadAsAsync<ProductViewModel>().Result;

                return PartialView("_Details", product);
            }
            Create();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/" + id).Result;

            var product = response.Content.ReadAsAsync<CreateProductModel>().Result;

            GetColorsAndCategoriesForEditMethod(product);

            return View(product);
        }

        private ActionResult GetColorsAndCategoriesForEditMethod(CreateProductModel product)
        {
            List<ColorViewModel> listColors = new List<ColorViewModel>();
            List<CategoryViewModel> listCategories = new List<CategoryViewModel>();

            HttpClient client = CreateHttpClient();

            HttpResponseMessage response = client.GetAsync("api/datebase/getcolors").Result;
            HttpResponseMessage responseCat = client.GetAsync("api/datebase/getcategories").Result;

            if (response.IsSuccessStatusCode && responseCat.IsSuccessStatusCode)
            {
                listColors = response.Content.ReadAsAsync<List<ColorViewModel>>().Result;
                listCategories = responseCat.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
            }

            ViewBag.ColorId = new SelectList(listColors, "Id", "Name", product.ColorId = product.Color.Id);
            ViewBag.CategoryId = new SelectList(listCategories, "Id", "Name", product.CategoryId = product.Category.Id);

            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id,[Bind(Include = "Name,Description,ColorId,CategoryId,Price")] CreateProductModel model)
        {
            if (ModelState.IsValid)
            {
                Put(id, model);
                return RedirectToAction("Index"); 
            }
            Create();

            return View();
        }

        public ActionResult Delete(int id)
        {
                if (ModelState.IsValid)
                {
                    HttpClient client = CreateHttpClient();

                    HttpResponseMessage response = client.DeleteAsync("api/intranet/delete-product/" + id).Result;

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

        public string Post(CreateProductModel model)
        {
            HttpClient client = CreateHttpClient();

            var param = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/intranet/create-product/", content).Result;

            return response.Content.ToString();
        }

        public string Put(int id, CreateProductModel model)
        {
            HttpClient client = CreateHttpClient();

            var paramtwo = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            HttpContent contentTwo = new StringContent(paramtwo, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync("api/intranet/edit-product/" + id, contentTwo).Result;

            return response.Content.ToString();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("../Images"), fileName);
                file.SaveAs(path);
            }
            return View();
        }





        public FileResult GetImage(string fileName)
        {
            string imagePath = Directory.GetParent(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath).Parent.FullName + @"\Images";

            var path = Path.Combine(imagePath, fileName);

            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpg");
        }

        public FilePathResult GetFileFromDisk(string fileName)
        {
            string imagePath = Directory.GetParent(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath).Parent.FullName + @"\Images";
            //string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
            //string fileName = "test.txt";
            return File(imagePath + fileName, "image/jpg");
        }

        

    }
}