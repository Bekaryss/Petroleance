using FanArt.Web.HtmlHelper;
using Petroleance.Models;
using Petroleance.Models.FanModels;
using Petrolence.Web.HtmlHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Petroleance.Controllers.FanAdmin
{
    [Authorize(Roles = "Administrator")]
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        int _id;
        Models.FanModels.Image image = new Models.FanModels.Image();
        SystemImage simage = new SystemImage();
        public ActionResult ImageList(int Id)
        {
            ViewBag.AlbumId = Id;
            var image = db.Images.ToList().Where(a => a.AlbumId == Id);
            return View(image);
        }
        // GET: Image
        public ActionResult Create(int Id)
        {
            ViewBag.AlbumId = Id;
            return View();
        }
        [HttpPost]
        public JsonResult ImageCreate()
        {
            HttpPostedFileBase myFile = Request.Files["file"];
            var name = Request.Form[0];
            var alt = Request.Form[1];
            var a = 0;
            var x = 0;
            var y = 0;
            var w = 0;
            var h = 0;
            bool isHaveParam = false;
            bool isUploaded = false;
            if (Request.Form[3] == "" && Request.Form[4] == "" && Request.Form[5] == "" && Request.Form[6] == "")
            {
                a = Int32.Parse(Request.Form[2]);
            }
            else
            if (Request.Form[2] != "" && Request.Form[3] != "" && Request.Form[4] != "" && Request.Form[5] != "" && Request.Form[6] != "")
            {
                isHaveParam = true;
                a = Int32.Parse(Request.Form[2]);
                x = Int32.Parse(Request.Form[3]);
                y = Int32.Parse(Request.Form[4]);
                w = Int32.Parse(Request.Form[5]);
                h = Int32.Parse(Request.Form[6]);

            }

            string ImageName = Path.GetFileNameWithoutExtension(myFile.FileName);
            string ImageExtension = Path.GetExtension(myFile.FileName);

            DateTime date = DateTime.Now;
            string d = date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
            string u = User.Identity.Name;
            string FolderPath = FileHelper.GetAlbumImagePath(u, a, d);
            string FolderDirectory = FileHelper.GetDirectoryImageFolder(FolderPath);

            if (myFile != null && myFile.ContentLength != 0)
            {
                if (FileHelper.CreateFolderIfNeeded(FolderDirectory))
                {
                    if (name == "")
                    {
                        myFile.SaveAs(Path.Combine(FolderDirectory, ImageName + ImageExtension));
                        _id = Save.SaveChen(ImageName, ImageExtension, FolderPath, date, alt, a);
                    }
                    else
                    {
                        myFile.SaveAs(Path.Combine(FolderDirectory, name + ImageExtension));
                        _id = Save.SaveChen(name, ImageExtension, FolderPath, date, alt, a);
                    }
                    isUploaded = true;
                }

                image = Save.GetImageById(_id);

                string ImageFullPath = FileHelper.GetImagePath(image.FilePath, image.Name, image.Extension);

                byte[] cImage = System.IO.File.ReadAllBytes(Server.MapPath(ImageFullPath));
                byte[] croppedImage = cImage;
                if (isHaveParam == true)
                {
                    croppedImage = ImageHelper.CropImage(cImage, x, y, w, h);
                }

                string Lsize = "l";
                string Msize = "m";
                string Ssize = "s";

                byte[] largeImage = ImageHelper.ResizeImageFree(croppedImage, Lsize);
                byte[] midleImage = ImageHelper.ResizeImageFree(croppedImage, Msize);
                byte[] smallImage = ImageHelper.ResizeImageFree(croppedImage, Ssize);

                FileHelper.SaveFile(largeImage, Path.Combine(FolderPath + "/" + image.Name + "large" + image.Extension));
                FileHelper.SaveFile(midleImage, Path.Combine(FolderPath + "/" + image.Name + "middle" + image.Extension));
                FileHelper.SaveFile(smallImage, Path.Combine(FolderPath + "/" + image.Name + "small" + image.Extension));


            }
            string SendFilePath = image.FilePath.Replace("~", "") + "/" + image.Name + "middle" + image.Extension;
            return Json(new { isUploaded, SendFilePath, _id }, "text/html");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.FanModels.Image images = db.Images.Find(id);
            if (images == null)
            {
                return HttpNotFound();
            }
            return View(images);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.FanModels.Image image = db.Images.Find(id);
            int albumId = image.AlbumId;
            db.Images.Remove(image);
            db.SaveChanges();
            FileHelper.DeleteFolder(image.FilePath);
            return RedirectToAction("ImageList", new { Id = albumId });
        }
        Size Lsize;
        Size Msize;
        Size Ssize;

        [HttpPost]
        public JsonResult SystemImageCreate()
        {
            bool isUploaded = false;
            HttpPostedFileBase myFile = Request.Files["file"];
            var alt = "Image";
            var x = 0;
            var y = 0;
            var w = 0;
            var h = 0;
            string type = "";
            bool isHaveParam = false;


            if (Request.Form[0] != "" && Request.Form[1] != "" && Request.Form[2] != "" && Request.Form[3] != "")
            {
                isHaveParam = true;
                x = Int32.Parse(Request.Form[0]);
                y = Int32.Parse(Request.Form[1]);
                w = Int32.Parse(Request.Form[2]);
                h = Int32.Parse(Request.Form[3]);
            }
            if (Request.Form.Count == 5)
            {
                type = Request.Form[4];
            }

            string ImageName = Path.GetFileNameWithoutExtension(myFile.FileName);
            string ImageExtension = Path.GetExtension(myFile.FileName);
            //Date
            DateTime date = DateTime.Now;
            string d = date.ToString().Replace(".", "-").Replace(" ", "-").Replace(":", "-");
            //User Name
            string u = User.Identity.Name;
            //Image Name 
            string FolderPath = FileHelper.GetSysImagePath(u, d);
            string FolderDirectory = FileHelper.GetDirectoryImageFolder(FolderPath);


            if (myFile != null && myFile.ContentLength != 0)
            {
                if (FileHelper.CreateFolderIfNeeded(FolderDirectory))
                {
                    //Save Image
                    myFile.SaveAs(Path.Combine(FolderDirectory, ImageName + ImageExtension));
                    //Save Db And Get Id
                    _id = Save.SaveChenSys(ImageName, ImageExtension, FolderPath, date, alt);

                    isUploaded = true;
                }
                //Get Image
                simage = Save.GetSysImageById(_id);
                //GEt Full Path
                string ImageFullPath = FileHelper.GetImagePath(simage.FilePath, simage.Name, simage.Extension);


                byte[] cImage = System.IO.File.ReadAllBytes(Server.MapPath(ImageFullPath));
                byte[] croppedImage = cImage;
                if (isHaveParam == true)
                {
                    croppedImage = ImageHelper.CropImage(cImage, x, y, w, h);
                }
                if (type == "book")
                {
                    Lsize = new Size(720, 1080);
                    Msize = new Size(420, 630);
                    Ssize = new Size(240, 360);
                }
                else
                if (type == "article")
                {
                    Lsize = new Size(900, 360);
                    Msize = new Size(525, 210);
                    Ssize = new Size(300, 120);
                }
                else
                {
                    Lsize = new Size(1280, 720);
                    Msize = new Size(820, 405);
                    Ssize = new Size(320, 180);
                }
                byte[] largeImage = ImageHelper.ResizeImage(croppedImage, Lsize);
                byte[] midleImage = ImageHelper.ResizeImage(croppedImage, Msize);
                byte[] smallImage = ImageHelper.ResizeImage(croppedImage, Ssize);

                FileHelper.SaveFile(largeImage, Path.Combine(FolderPath + "/" + simage.Name + "large" + simage.Extension));
                FileHelper.SaveFile(midleImage, Path.Combine(FolderPath + "/" + simage.Name + "middle" + simage.Extension));
                FileHelper.SaveFile(smallImage, Path.Combine(FolderPath + "/" + simage.Name + "small" + simage.Extension));
            }
            string SendFilePath = simage.FilePath.Replace("~", "") + "/" + simage.Name + "middle" + simage.Extension;
            return Json(new { isUploaded, SendFilePath, _id }, "text/html");
        }
    }
}