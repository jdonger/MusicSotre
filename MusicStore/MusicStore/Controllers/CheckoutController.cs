using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class CheckoutController : Controller
    {
        MusicStoreEntity db = new MusicStoreEntity();
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SubmitOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitOrder(Orders order)
        {
            if (ModelState.IsValid)//判断数据验证是否通过
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                db.Orders.Add(order);
                db.SaveChanges();

                var cartItems=db.Carts.Where(p => p.CartId == User.Identity.Name).ToList();

                decimal total=0;

                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetails()
                    {
                        AlbumId=item.AlbumId,
                        OrderId=order.OrderId,
                        UnitPrice=item.Albums.Price,
                        Quantity=item.Count
                    };

                    db.OrderDetails.Add(orderDetail);

                    total += item.Count * item.Albums.Price;
                }

                order.Total = total;

                db.Carts.RemoveRange(cartItems);//删除购物车
                db.SaveChanges();

                return RedirectToAction("Complete",new {id=order.OrderId });
            }

            return View(order);
        }

        public ActionResult Complete(int id)
        {
            ViewBag.OrderID = id;
            return View();
        }

    }
}