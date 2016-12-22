using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petroleance.Models.FanModels
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string MusicPath { get; set; }
        public string Author { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int? SImageId { get; set; }
        public virtual SystemImage SImage { get; set; }
    }
}