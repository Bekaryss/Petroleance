using FanArt.Web.HtmlHelper;
using Microsoft.AspNet.Identity;
using Petroleance.Models;
using Petroleance.Models.FanModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Petroleance.Controllers.FanAdmin
{
    [Authorize(Roles = "Administrator")]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.SImage).Include(a => a.User);
            return View(articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Articles/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,SImageId")] Article article, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                DateTime date = DateTime.Now;
                string d = date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
                string u = User.Identity.Name;
                string FolderPath = FileHelper.GetMusicPath(u, d);
                string FolderDirectory = FileHelper.GetDirectoryImageFolder(FolderPath);

                string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                string FileExtension = Path.GetExtension(file.FileName);

                if (FileHelper.CreateFolderIfNeeded(FolderDirectory))
                {
                    file.SaveAs(Path.Combine(FolderDirectory, FileName + FileExtension));
                }
                article.MusicPath = FolderPath + "/" + FileName + FileExtension;
                article.Date = date;
                article.UserId = User.Identity.GetUserId();
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", article.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", article.UserId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", article.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", article.UserId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] Article article, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var artic = (from m in db.Articles where m.Id == article.Id select m).First();
                DateTime date = DateTime.Now;
                if (file != null)
                {
                    string d = date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
                    string u = User.Identity.Name;
                    string FolderPath = FileHelper.GetMusicPath(u, d);
                    string FolderDirectory = FileHelper.GetDirectoryImageFolder(FolderPath);

                    string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string FileExtension = Path.GetExtension(file.FileName);

                    if (FileHelper.CreateFolderIfNeeded(FolderDirectory))
                    {
                        file.SaveAs(Path.Combine(FolderDirectory, FileName + FileExtension));
                        string dd = artic.Date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
                        string DelFolderPath = FileHelper.GetMusicPath(u, dd);
                        FileHelper.DeleteFolder(DelFolderPath);
                    }
                    artic.MusicPath = FolderPath + "/" + FileName + FileExtension;

                }
                artic.Date = date;
                artic.Title = article.Title;
                artic.Content = article.Content;
                db.Entry(artic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SImageId = new SelectList(db.SImages, "Id", "Name", article.SImageId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", article.UserId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);

            SystemImage image = db.SImages.Find(article.SImageId);
            FileHelper.DeleteFolder(image.FilePath);
            db.SImages.Remove(image);
            string u = User.Identity.Name;
            string dd = article.Date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
            string DelFolderPath = FileHelper.GetMusicPath(u, dd);
            FileHelper.DeleteFolder(DelFolderPath);


            db.Articles.Remove(article);
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