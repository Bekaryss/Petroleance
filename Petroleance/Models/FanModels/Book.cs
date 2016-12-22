using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petroleance.Models.FanModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}