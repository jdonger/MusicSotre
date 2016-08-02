using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        MusicStoreEntity db = new MusicStoreEntity();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cartList=db.Carts.Where(a => a.CartId == User.Identity.Name).ToList();

            decimal total = 0;
            foreach (var item in cartList)
            {
                total += item.Count * item.Albums.Price;
            }

            ViewBag.CartTotal = total;
            return View(cartList);
        }

        public ActionResult AddToCart(int id)
        {
            var album = db.Albums.Find(id);
            if (album != null)
            {
                var cartItem= db.Carts.SingleOrDefault(p => p.AlbumId == id);
                if (cartItem != null)
                {
                    cartItem.Count++;
                }
                else
                {
                    cartItem = new Carts()
                    {
                        AlbumId = id,
                        CartId = User.Identity.Name,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    db.Carts.Add(cartItem);
                }
                db.SaveChanges();
            }
            //Response.Write("<script>成功添加到购物车!</script>");
            return RedirectToAction("Index");
        }
    }
}