using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petroleance.Models
{
    public class Statistics
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public int CorrectAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public double Percentage { get; set; }

        public virtual TriviaTest TriviaTests { get; set; }
    }
}