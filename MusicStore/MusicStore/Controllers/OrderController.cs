using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        MusicStoreEntity db = new MusicStoreEntity();   
        // GET: Order
        public ActionResult Index()
        {
            var list=db.Orders.Where(p => p.Username == User.Identity.Name).ToList();
            return View(list);
        }

        public ActionResult Details(int orderID)
        {
            var list = db.OrderDetails.Where(p => p.OrderId == orderID).ToList();
            return View(list);
        }
    }
}