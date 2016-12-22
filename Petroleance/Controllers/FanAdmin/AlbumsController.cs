using Microsoft.AspNet.Identity;
using Petroleance.Models;
using Petroleance.Models.FanModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FanArt.Web.HtmlHelper;

namespace Petroleance.Controllers.FanAdmin
{
    [Authorize(Roles = "Administrator")]
    public class AlbumsController : Controller
    {
        // GET: Albums
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            var albums = db.Albums;
            return View(albums.ToList().Where(a => a.UserId == id));
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("ImageList", "Image", new { Id = id });
        }

        // GET: Albums/Create
        public ActionResult Create(string userid)
        {
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View(userid);
        }

        // POST: Albums/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumId,Name,SImageId")] Album album)
        {
            if (ModelState.IsValid)
            {
                album.UserId = User.Identity.GetUserId();
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", album.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", album.UserId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", album.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", album.UserId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumId,Name")] Album album)
        {
            var albu = (from m in db.Albums where m.AlbumId == album.AlbumId select m).First();
            if (ModelState.IsValid)
            {
                albu.Name = album.Name;
                db.Entry(albu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", album.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", album.UserId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);

            SystemImage image = db.SImages.Find(album.SImageId);
            string dd = image.Date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
            string u = User.Identity.Name;
            string DelFolderPath = FileHelper.GetSysImagePath(u, dd);
            FileHelper.DeleteFolder(DelFolderPath);
            db.SImages.Remove(image);

            db.Albums.Remove(album);
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