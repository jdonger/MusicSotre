using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;

using X.PagedList;

namespace MusicStore.Controllers
{
    [Authorize(Roles ="admin")]
    public class StoreManagerController : Controller
    {
        private MusicStoreEntity db = new MusicStoreEntity();

        // GET: StoreManager
        public ActionResult Index(int page=1)
        {
            var albums = db.Albums.Include(a => a.Artists).Include(a => a.Genres)
                .OrderByDescending(a=>a.AlbumId);
            return View(albums.ToPagedList(page,30));
        }

        // GET: StoreManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        // GET: StoreManager/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            return View();
        }

        // POST: StoreManager/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Albums albums,
            HttpPostedFileBase imageFile)//添加参数
        {
            if (ModelState.IsValid)
            {
                if(imageFile != null)
                {
                    string guid=Guid.NewGuid().ToString();
                    string imageName =guid+ Path.GetFileName(imageFile.FileName);//使用Path需using System.IO;
                    string serverPath = Server.MapPath("~/Content/Images/"+imageName);
                    imageFile.SaveAs(serverPath);

                    albums.AlbumArtUrl = "/Content/Images/"+imageName;
                }
                db.Albums.Add(albums);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", albums.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", albums.GenreId);
            return View(albums);
        }

        // GET: StoreManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", albums.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", albums.GenreId);
            return View(albums);
        }

        // POST: StoreManager/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Albums albums)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albums).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", albums.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", albums.GenreId);
            return View(albums);
        }

        // GET: StoreManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Albums albums = db.Albums.Find(id);
            db.Albums.Remove(albums);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
