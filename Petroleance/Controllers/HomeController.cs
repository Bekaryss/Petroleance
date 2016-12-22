using Microsoft.AspNet.Identity;
using Petroleance.Models;
using Petroleance.Models.FanModels;
using Petroleance.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Petroleance.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TriviaContext bb = new TriviaContext();
        public ActionResult Index()
        {
            ViewBag.Aticles = db.Articles.Include(b => b.SImage).Include(b => b.User).OrderByDescending(d => d.Date).Take(8);
            ViewBag.image = db.Images.OrderByDescending(d => d.Date).Take(20);
            var Story = db.Articles.Include(b => b.SImage).Include(b => b.User).Where(s => s.Id == 5).ToList();
            return View();
        }
        public ActionResult Gallery()
        {
            var image = db.Images.OrderByDescending(d => d.Date);
            return View(image.ToList());
        }
        public ActionResult News()
        {
            var article = db.Articles.Include(b => b.SImage).Include(b => b.User).OrderByDescending(d => d.Date);
            return View(article.ToList());
        }
        public ActionResult NewsId(int? id)
        {
            var article = db.Articles.Include(b => b.SImage).Include(b => b.User).Where(s => s.Id == id).ToList();
            return View(article);
        }
        [Authorize]
        public ActionResult Test()
        {
            var test = bb.TriviaTest.ToList();
            return View(test);
        }
        [Authorize]
        public ActionResult TestId(int? id)
        {
            TriviaContext tc = new TriviaContext();
            
            var test = tc.TriviaQuestions.Include(a => a.Options).Where(b => b.TestId == id).ToList();
            ViewBag.TestTitle = tc.TriviaTest.Where(b => b.Id == id).First().Title;
            return View(test);
        }
        [Authorize]
        public ActionResult Statistics()
        {
            string Uid = User.Identity.GetUserId();
            var statistics = bb.Statisticss.Where(b => b.UserId == Uid).ToList();
            ViewBag.TestTitle = bb.TriviaTest.ToList();
            return View(statistics);
        }
        public ActionResult UserRating(int? Id)
        {
            var rating = bb.Statisticss.Where(a => a.TestId == Id).OrderByDescending(c => c.Percentage).ToList();
            ViewBag.User = db.Users.ToList();
            return View(rating);
        }
        [HttpPost]
        public ActionResult CheckTest()
        {
            if(Request.Form.Count == 0)
            {
                return RedirectToAction("BadTest");
            }
            TriviaAnswer ta = new TriviaAnswer();
            Statistics stat = new Statistics();
            string Uid = User.Identity.GetUserId();
            int OptionListCount = Request.Form.Count;
            int n = 0;
  
            int id = 0;
            var tOption = new TriviaOption();
            while(n < OptionListCount)
            {         
                id = int.Parse(Request.Form[n]);
                tOption = bb.TriviaOptions.Where(d => d.Id == id).FirstOrDefault();
                ta.OptionId = id;
                ta.QuestionId = tOption.QuestionId;
                ta.TestId = tOption.TriviaQuestion.TestId;
                ta.UserId = Uid;
                var mta = bb.TriviaAnswers.Where(g => g.QuestionId == tOption.QuestionId).Where(h => h.UserId == Uid).ToList();
                if (mta != null)
                {
                    foreach(var i in mta)
                    {
                        bb.TriviaAnswers.Remove(i);
                    }           
                }
                bb.TriviaAnswers.Add(ta);
                bb.SaveChanges();
                n++;
            }            
            var tta = bb.TriviaAnswers.Where(a => a.TestId == ta.TestId).Where(b => b.UserId == Uid).ToList();
            int c = 0;
            int w = 0;
            foreach (var item in tta)
            {
                var ttOption = bb.TriviaOptions.Where(a => a.Id == item.OptionId).First().IsCorrect;
                if (ttOption == true)
                {
                    c++;
                }
                else
                {
                    w++;
                }
            }
            stat.TestId = ta.TestId;
            stat.UserId = Uid;
            stat.CorrectAnswer = c;
            stat.WrongAnswer = w;
            double total = c + w;
            stat.Percentage = Math.Floor((c / total) * 100);
            var mstat = bb.Statisticss.Where(g => g.TestId == stat.TestId).Where(h => h.UserId == Uid).FirstOrDefault();
            if (mstat != null)
            {
                bb.Statisticss.Remove(mstat);
                bb.SaveChanges();
            }
            bb.Statisticss.Add(stat);         
            bb.SaveChanges();
            return RedirectToAction("ShowResult", new { StatId = stat.Id});
        }
        [Authorize]
        public ActionResult BadTest()
        {
            return View();
        }
        [Authorize]
        public ActionResult ShowResult(int StatId)
        {
            var Statis = bb.Statisticss.Find(StatId);                      
            return View(Statis);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}