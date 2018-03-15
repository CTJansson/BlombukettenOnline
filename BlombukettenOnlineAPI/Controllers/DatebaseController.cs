using BlombukettenOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlombukettenOnlineAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DatebaseController : ApiController
    {
        public IEnumerable<ProductModel> GetAllProducts()
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Products.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Color = ctx.Colors.Where(c => c.Products.Any(co => co.ColorId == p.ColorId)).Select(c => new ColorModel { Id = c.Id, Name = c.Name }).FirstOrDefault(),
                Category = ctx.Categories.Where(c => c.Products.Any(co => co.CategoryId == p.CategoryId)).Select(c => new CategoryModel { Id= c.Id, Name = c.Name, ParentId = c.ParentId}).FirstOrDefault(),
                Price = p.Price,
                Image = p.Picture,
                Active = p.Active,
             
            }).Where(pr => pr.Active == true).ToList();
        }

        [Route("api/datebase/filterproducts/{categoryId}/{colorId}")]
        public IEnumerable<ProductModel> GetFilteredProducts(int categoryId, int colorId)
        {
            var temp = GetAllProducts();

            var filteredListOfProducts = new List<ProductModel>();

            if (categoryId > 0)
            {
                foreach (var product in temp)
                {
                    if (product.CategoryId == categoryId || product.CategoryId == categoryId)
                    {
                        if (colorId > 0 && product.Color.Id == colorId)
                        {
                            filteredListOfProducts.Add(product);
                        }
                        else if (colorId == 0)  
                        {
                            filteredListOfProducts.Add(product);
                        }
                    }
                }
            }

            return filteredListOfProducts;
        }

        [Route("api/datebase/getxrandomproducts/{amount}")]
        public IEnumerable<ProductModel> GetXRandomProducts(int amount)
        {
            Random rng = new Random();

            var dbList = GetAllProducts();
            var numbers = new List<int>();
            var tenProducts = new List<ProductModel>();

            if (amount <= dbList.Count())
            {
                for (int i = 0; i < amount; i++)
                {
                    int number = rng.Next(0, dbList.Count());

                    if (!numbers.Contains(number))
                    {
                        numbers.Add(number);

                        tenProducts.Add(dbList.ElementAtOrDefault(number));
                    }
                    else
                        i--;
                }
            }
            else
                return dbList;

            return tenProducts;
        }

        [Route("api/datebase/{id}")]
        public ProductModel GetProduct([FromUri]int id) 
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Products.Where(p => p.Id == id).Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Color = ctx.Colors.Where(c => c.Products.Any(co => co.ColorId == p.ColorId)).Select(c => new ColorModel { Id = c.Id, Name = c.Name }).FirstOrDefault(),
                Category = ctx.Categories.Where(c => c.Products.Any(pr => pr.Id == p.Id)).Select(c => new CategoryModel { Id = c.Id, Name = c.Name, ParentId = c.ParentId }).FirstOrDefault(),
                Price = p.Price,
                Image = p.Picture,
            }).FirstOrDefault();
        }

        [Route("api/datebase/getchosenproducts/{categoryIds}/{colorIds}")]
        public IEnumerable<ProductModel> GetChosenProducts(string categoryIds, string colorIds)
        {
            string[] catIds = categoryIds.Split(',');
            string[] colIds = colorIds.Split(',');

            var dbList = GetAllProducts();
            var listOfProducts = new List<ProductModel>();

            foreach (var categoryId in catIds)
            {
                foreach (var colorId in colIds)
                {
                    var items = dbList.Where(db => db.Category.Id == int.Parse(categoryId) || db.Category.ParentId == int.Parse(categoryId)).Where(db => db.Color.Id == int.Parse(colorId));
                    listOfProducts.AddRange(items);
                }
            }

            return listOfProducts;
        }

        [Route("api/datebase/getcategories")]
        public IEnumerable<CategoryModel> GetCategories() 
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Categories.Where(q => q.Active == true).Select(c => new CategoryModel 
            {
                Id = c.Id,
                Name = c.Name,           
                ParentId = c.ParentId,
            }).ToList(); 
        }

        [Route("api/datebase/getcolors")]
        public IEnumerable<ColorModel> GetColors()
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Colors.Where(q => q.Active == true).Select(c => new ColorModel
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
        [Route("api/datebase/color/{id}")]
        public ColorModel GetColor(int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Colors.Where(c => c.Id == id).Select(q => new ColorModel { Id = q.Id, Name = q.Name }).FirstOrDefault();
        }

        [Route("api/datebase/category/{id}")]
        public CategoryModel GetCategory(int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Categories.Where(c => c.Id == id).Select(q => new CategoryModel { Id = q.Id, Name = q.Name, ParentId = q.ParentId }).FirstOrDefault();
        }
    }
}
