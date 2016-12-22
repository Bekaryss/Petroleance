using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petroleance.Models.FanModels
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }

        public int? SImageId { get; set; }
        public virtual SystemImage SImage { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual List<Image> Images { get; set; }
    }
}