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
    public class IntranetController : ApiController
    {
        [Route("api/intranet/get-orders/")]
        public IEnumerable<OrderModel> GetOrders()
        {
            var ctx = new DAL.Grupptenta4Entities();

            return ctx.Orders.Select(o => new OrderModel
            {
                Id = o.Id,
                Comment = o.Comment,
                TotalPrice = o.TotalPrice,
                IsDelivered = o.IsDelivered,
                Email = new EmailModel
                {
                    Email = o.Email.Email1
                },
                Address = new AddressModel
                {
                    Id = o.Address.Id,
                    PostalCode = o.Address.PostalCode,
                    Street = o.Address.Street
                },
                Delivery = ctx.DeliveryMethods.Select(dm => new DeliveryModel { Id = dm.Id, Method = dm.Method }).FirstOrDefault(dm => dm.Id == o.DeliveryId),
                Payment = ctx.PaymentMethods.Select(pm => new PaymentModel { Id = pm.Id, Method = pm.Method }).FirstOrDefault(pm => pm.Id == o.PaymentId),
                Store = ctx.Stores.Select(s => new StoreModel { Id = s.Id, Name = s.Name }).FirstOrDefault(s => s.Id == o.StoreId),
            }).OrderByDescending(o => o.Id);
        }

        [Route("api/intranet/get-order/{id}")]
        public OrderModel GetOrder([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();
            var order = ctx.Orders.Find(id);
            var orderBouquets = order.Boquetts.Where(b => b.OrderId == order.Id);

            ICollection<BoquetteModel> list = orderBouquets.Select(ob => new BoquetteModel { Id = ob.Id, Quantity = ob.Quantity, Sum = ob.Sum, Products = ctx.Products.Where(p => p.ProductBoquetts.Any(pb => pb.BoquettId == ob.Id)).ToList().Select(p => new ProductModel { Id = p.Id, Name = p.Name, ColorId = p.ColorId, CategoryId = p.CategoryId, Description = p.Description, Price = p.Price, Active = p.Active, Quantity = p.ProductBoquetts.FirstOrDefault(pbb => pbb.BoquettId == ob.Id && pbb.ProductId == p.Id).Quantity }).ToList() }).ToList();

            return new OrderModel()
            {
                Id = order.Id,
                Buquettes = list.ToList(), 
                Address = ctx.Addresses.Select(a => new AddressModel { Id = a.Id, PostalCode = a.PostalCode, Street = a.Street }).FirstOrDefault(a => a.Id == order.Address.Id),
                Comment = order.Comment,
                TotalPrice = order.TotalPrice,
                Delivery = ctx.DeliveryMethods.Select(dm => new DeliveryModel { Id = dm.Id, Method = dm.Method }).FirstOrDefault(dm => dm.Id == order.DeliveryId),
                Payment = ctx.PaymentMethods.Select(pm => new PaymentModel { Id = pm.Id, Method = pm.Method }).FirstOrDefault(pm => pm.Id == order.PaymentId),
                Store = ctx.Stores.Select(s => new StoreModel { Id = s.Id, Name = s.Name }).FirstOrDefault(s => s.Id == order.StoreId),
                Email = ctx.Emails.Select(e => new EmailModel { Id = e.Id, Email = e.Email1, Subscribe = e.Subscribe }).FirstOrDefault(e => e.Id == order.EmailId),
            };


        }

        [Route("api/intranet/delete-order/{id}")]
        public void DeleteOrder([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            var orderToBeRemoved = ctx.Orders.Find(id);
            var orderBouqet = orderToBeRemoved.Boquetts.Where(pb => pb.OrderId == orderToBeRemoved.Id);

            var transaction = ctx.Database.BeginTransaction();
            try
            {
                var listOfJunc = new List<DAL.ProductBoquett>();

                foreach (var item in orderBouqet)
                {
                    foreach (var prod in item.ProductBoquetts)
                    {
                        var temp = ctx.ProductBoquetts.Where(pb => pb.Boquett.Id == item.Id).First();
                        listOfJunc.Add(temp);
                    }
                }

                ctx.ProductBoquetts.RemoveRange(listOfJunc);
                ctx.Boquetts.RemoveRange(orderToBeRemoved.Boquetts);
                ctx.Orders.Remove(orderToBeRemoved);
                ctx.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        [Route("api/intranet/edit-order/{id}")]
        public void Put([FromBody]OrderModel model, [FromUri]int id) // Måste ses över mer
        {
            var ctx = new DAL.Grupptenta4Entities();
            var orderToEdit = ctx.Orders.Find(id);

            orderToEdit.Comment = model.Comment;
            orderToEdit.TotalPrice = model.TotalPrice;

            // Editera databasens buketter till modellens uppdaterade buketter
            foreach (var dbBouquet in orderToEdit.Boquetts)
            {
                dbBouquet.Quantity = model.Buquettes.First(b => b.Id == dbBouquet.Id).Quantity;

                foreach (var dbProduct in dbBouquet.ProductBoquetts)
	            {
                    dbProduct.Quantity = model.Buquettes.First(b => b.Id == dbBouquet.Id).Products.First(p => p.Id == dbProduct.ProductId).Quantity;
	            }
            }

            // Räkna ut nya bukett priset
            decimal bouquetPrice = 0;

            foreach (var dbBouquet in orderToEdit.Boquetts)
            {
                foreach (var dbProduct in dbBouquet.ProductBoquetts)
                {
                    var productTemp = ctx.Products.FirstOrDefault(p => p.Id == dbProduct.ProductId);
                    bouquetPrice += productTemp.Price * dbProduct.Quantity;
                }
                dbBouquet.Sum = bouquetPrice;
                bouquetPrice = 0;
            }
         
            orderToEdit.Email = ctx.Emails.FirstOrDefault(e => e.Email1 == model.Email.Email) ?? (new DAL.Email { Email1 = model.Email.Email, Subscribe = model.Email.Subscribe });
            orderToEdit.Address = ctx.Addresses.FirstOrDefault(a => a.Street == model.Address.Street && a.PostalCode == model.Address.PostalCode) ?? (new DAL.Address { Street = model.Address.Street, PostalCode = model.Address.PostalCode });

            ctx.SaveChanges();
        }

        [Route("api/intranet/deliver-order/{id}")]
        public void Put([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();
            var orderToEdit = ctx.Orders.Find(id);

            orderToEdit.IsDelivered = true;
            ctx.SaveChanges();
        }

        [HttpPost]
        [Route("api/intranet/create-product/")]
        public void PostProduct([FromBody]CreateProductModel product) 
        {
            var ctx = new DAL.Grupptenta4Entities();
            product.Active = true;
            
            ctx.Products.Add(new DAL.Product
            {
                Name = product.Name,
                Description = product.Description,
                ColorId = product.ColorId,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Picture = product.Image,
                Active = product.Active,
            });

            ctx.SaveChanges();
        }

        [HttpDelete]
        [Route("api/intranet/delete-product/{id}")]
        public void DeleteProduct([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            var productToBeRemoved = ctx.Products.Find(id);
            productToBeRemoved.Active = false;

            ctx.SaveChanges();
        }

        [Route("api/intranet/edit-product/{id}")]
        public void Put([FromBody]CreateProductModel product, [FromUri]int id) // Måste ses över mer
        {
            var ctx = new DAL.Grupptenta4Entities();
            var productToEdit = ctx.Products.Find(id);

            productToEdit.Name = product.Name;
            productToEdit.Description = product.Description;
            productToEdit.ColorId = product.ColorId;
            productToEdit.CategoryId = product.CategoryId;
            productToEdit.Price = product.Price;

            ctx.SaveChanges();
        }

        [Route("api/intranet/create-category/")]
        public void PostCategory([FromBody] CategoryModel category)
        {
            var ctx = new DAL.Grupptenta4Entities();

            ctx.Categories.Add(new DAL.Category
            {
                Name = category.Name,
                ParentId = category.ParentId,
                Active = true,
            });

            ctx.SaveChanges();
        }

        [Route("api/intranet/create-color/")]
        public void PostColor([FromBody] ColorModel color)
        {
            var ctx = new DAL.Grupptenta4Entities();

            ctx.Colors.Add(new DAL.Color
            {
                Name = color.Name,
                Active = true,
            });

            ctx.SaveChanges();
        }

        [Route("api/intranet/edit-color/{id}")]
        public void Put([FromBody]ColorModel color, [FromUri]int id) // Måste ses över mer
        {
            var ctx = new DAL.Grupptenta4Entities();
            var colorToEdit = ctx.Colors.Find(id);

            colorToEdit.Name = color.Name;

            ctx.SaveChanges();
        }

        [Route("api/intranet/edit-category/{id}")]
        public void Put([FromBody]CategoryModel category, [FromUri]int id) // Måste ses över mer
        {
            var ctx = new DAL.Grupptenta4Entities();
            var categoryToEdit = ctx.Categories.Find(id);

            categoryToEdit.Name = category.Name;
            categoryToEdit.ParentId = categoryToEdit.ParentId;

            ctx.SaveChanges();
        }

        [HttpDelete]
        [Route("api/intranet/delete-category/{id}")]
        public void DeleteCategory([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            var categoryToBeRemoved = ctx.Categories.Find(id);
            categoryToBeRemoved.Active = false;

            ctx.SaveChanges();
        }

        [HttpDelete]
        [Route("api/intranet/delete-color/{id}")]
        public void DeleteColor([FromUri]int id)
        {
            var ctx = new DAL.Grupptenta4Entities();

            var colorToBeRemoved = ctx.Colors.Find(id);
            colorToBeRemoved.Active = false;

            ctx.SaveChanges();
        }
    }
}
