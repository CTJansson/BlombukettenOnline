using BlombukettenOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlombukettenOnlineAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : ApiController
    {
        private DAL.Grupptenta4Entities ctx;

        public OrderController()
        {
            ctx = new DAL.Grupptenta4Entities();
        }

        // http://localhost:50716/api/order/create-order/
        [Route("api/order/create-order/")]
        public void Post([FromBody]OrderModel model)
        {
            var transaction = ctx.Database.BeginTransaction();

            try
            {
                var newOrder = new DAL.Order
                {
                    Comment = model.Comment,
                    Email = ctx.Emails.FirstOrDefault(e => e.Email1 == model.Email.Email) ?? (new DAL.Email { Email1 = model.Email.Email, Subscribe = model.Email.Subscribe }),
                    Address = ctx.Addresses.FirstOrDefault(a => a.Street == model.Address.Street && a.PostalCode == model.Address.PostalCode) ?? (new DAL.Address { PostalCode = model.Address.PostalCode, Street = model.Address.Street }),
                    DeliveryId = model.DeliveryMethod,
                    PaymentId = model.PaymentMethod,
                    StoreId = model.StoreId,
                    IsDelivered = false,
                };

                decimal bouquetPrice = 0;
                decimal totalSum = 0;

                foreach (var bouquet in model.Buquettes)
                {
                    var tempBouquet = new DAL.Boquett { Id = bouquet.Id, Sum = bouquet.Sum, OrderId = newOrder.Id, Quantity = bouquet.Quantity };

                    foreach (var product in bouquet.Products)
                    {
                        var productTemp = ctx.Products.FirstOrDefault(p => p.Id == product.Id);

                        bouquetPrice += productTemp.Price * product.Quantity;
                        tempBouquet.Sum += bouquetPrice;

                        var ProductBouquetToDAL = new DAL.ProductBoquett() // Junction table mellan product och bouqet
                        {
                            Boquett = tempBouquet, // Bukettens ID
                            Product = ctx.Products.FirstOrDefault(p => p.Id == product.Id), // Produktens ID
                            Quantity = product.Quantity, // Antalet produkter i buketten
                        };
                        totalSum += bouquetPrice * bouquet.Quantity;
                        bouquetPrice = 0;

                        ctx.ProductBoquetts.Add(ProductBouquetToDAL);
                    }
                }

                newOrder.TotalPrice = totalSum;
                ctx.Orders.Add(newOrder);
                ctx.SaveChanges();

                transaction.Commit();

                model.Id = newOrder.Id;
                model.Payment = ctx.PaymentMethods.Select(p => new PaymentModel { Id = p.Id, Method = p.Method }).FirstOrDefault(q => q.Id == model.PaymentMethod);
                model.Delivery = ctx.DeliveryMethods.Select(d => new DeliveryModel { Id = d.Id, Method = d.Method }).FirstOrDefault(q => q.Id == model.DeliveryMethod);

                SendEpost(model, totalSum);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }

        public void SendEpost(OrderModel model, decimal totalPrice)
        {
            string smtpAddress = "smtp.live.com";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "skolanAldina@hotmail.com";
            string password = "teknikskolan89";
            string emailTo = model.Email.Email;
            int subjectinfo = model.Id;
            string paymentmethod = model.Payment.Method;
            string deliverimethod = model.Delivery.Method;

            using (MailMessage mail = new MailMessage())
            {
                string sBody = "";

                sBody += "Ordernummer:" + subjectinfo + "<br/>";
                sBody += "Pris: " + totalPrice + "<br/>";

                for (int i = 0; i < model.Buquettes.Count; i++)
                {
                    sBody += ("Bukett " + i + "(" + model.Buquettes.ToList()[i].Quantity + ")<br/>");

                    for (int j = 0; j < model.Buquettes.ToList()[i].Products.Count; j++)
                    {
                        sBody += ("   " + model.Buquettes.ToList()[i].Products.ToList()[j].Name + " - " + model.Buquettes.ToList()[i].Products.ToList()[j].Quantity + " st<br/>");
                    }
                }

                mail.Subject = "BlombukettenOnline - Order: #" + model.Id;
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Body = sBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }
    }
}
