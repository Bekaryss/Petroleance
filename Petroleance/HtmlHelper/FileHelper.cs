using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace FanArt.Web.HtmlHelper
{
    public class FileHelper
    {
        public static string GetDirectoryImageFolder(string _FolderPath)
        {
            string path = HttpContext.Current.Server.MapPath(_FolderPath);
            return path;
        }
        public static string GetAlbumImagePath(string username, int albumId, string FolderName)
        {
            string path = "/Content/Images/" + username + "/" + "Album-" + albumId.ToString() + "/" + FolderName;
            return path;
        }

        public static string GetSysImagePath(string username, string FolderName)
        {
            string path = "/Content/Images/" + username + "/SystemImage/" + FolderName;
            return path;
        }

        public static string GetImagePath(string FolderPath, string ImageName, string ImageExtension)
        {
            string path = FolderPath + "/" + ImageName + ImageExtension;
            return path;
        }

        public static string GetBooksPath(string username, string FolderName)
        {
            string path = "/Content/Books/" + username + "/" + FolderName;
            return path;
        }

        public static string GetMusicPath(string username, string FolderName)
        {
            string path = "/Content/Music/" + username + "/" + FolderName;
            return path;
        }

        public static void DeleteFolder(string _path)
        {
            string path = GetDirectoryImageFolder(_path);
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                Directory.Delete(path);
            }
        }
        public static void SaveFile(byte[] content, string path)
        {
            string filePath = GetFileFullPath(path);
            //Save file
            using (FileStream str = File.Create(filePath))
            {
                str.Write(content, 0, content.Length);
            }
        }

        public static string GetFileFullPath(string path)
        {
            string relName = path.StartsWith("~") ? path : path.StartsWith("/") ? string.Concat("~", path) : path;

            string filePath = relName.StartsWith("~") ? HostingEnvironment.MapPath(relName) : relName;

            return filePath;
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    //TODO: You must process this exception.
                    result = false;
                }
            }
            return result;
        }
    }
}