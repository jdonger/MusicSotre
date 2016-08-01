using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        MusicStoreEntity db = new MusicStoreEntity();
        public ActionResult Index()
        {
            
            List<Albums> list= db.Albums.OrderByDescending(a=>a.OrderDetails.Count).Take(12).ToList();
            return View(list);
        }

        public ActionResult Details(int id)
        {
            Albums a = db.Albums.Find(id);
            return View(a);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}