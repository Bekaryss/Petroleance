﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petroleance.Models.FanModels
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Alt { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }

        public int AlbumId { get; set; }
        public virtual Album Albums { get; set; }
    }
}