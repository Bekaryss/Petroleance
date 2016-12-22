

using Petroleance.Models;
using Petroleance.Models.FanModels;
using System;
using System.Data.Entity;
using System.Linq;

namespace Petrolence.Web.HtmlHelper
{
    public class Save
    {
        
        public static int SaveChen(string ImageName, string ImageExtension, string FolderPath, DateTime date, string alt, int albumid)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Image img = new Image();

            img.Name = ImageName;
            img.Extension = ImageExtension;
            img.FilePath = FolderPath;
            img.Date = date;
            img.Alt = alt;
            img.AlbumId = albumid;
            db.Images.Add(img);
            db.SaveChanges();
            return img.Id;
        }

        public static int SaveChenSys(string ImageName, string ImageExtension, string FolderPath, DateTime date, string alt)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemImage img = new SystemImage();

            img.Name = ImageName;
            img.Extension = ImageExtension;
            img.FilePath = FolderPath;
            img.Date = date;
            img.Alt = alt;
            db.SImages.Add(img);
            db.SaveChanges();
            return img.Id;
        }

        public static int SaveImage()
        {
            int a=45;
            return a;
        }

        public static int? SaveEdit(int? _id, string FolderName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Image current_img = db.Images.FirstOrDefault(p => p.Id == _id);

            foreach (Image image in db.Images.Where(i => i.Id == _id))
            {
                current_img.Name = image.Name;
                current_img.Extension = image.Extension;
                current_img.Alt = image.Alt;
                current_img.FilePath = FolderName;
                current_img.Date = DateTime.Now;
            }
            db.Entry(current_img).State = EntityState.Modified;
            db.SaveChanges();

            return _id;
        }

        public static Image GetImageById(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Image image = new Image();
            foreach (Image item in db.Images)
            {
                if (item.Id == id)
                {
                    image.Id = item.Id;
                    image.Name = item.Name;
                    image.Alt = item.Alt;
                    image.Date = item.Date;
                    image.Extension = item.Extension;
                    image.FilePath = item.FilePath;
                    image.AlbumId = item.AlbumId;
                }
            }
            return image;
        }

        public static SystemImage GetSysImageById(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemImage simage = new SystemImage();
            foreach (SystemImage item in db.SImages)
            {
                if (item.Id == id)
                {
                    simage.Name = item.Name;
                    simage.Alt = item.Alt;
                    simage.Date = item.Date;
                    simage.Extension = item.Extension;
                    simage.FilePath = item.FilePath;                  
                }
            }
            return simage;
        }
    }
}