using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Petroleance.Models;

namespace Petroleance.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TestsAdminController : Controller
    {
        private TriviaContext db = new TriviaContext();

        // GET: TestsAdmin
        public ActionResult Index()
        {
            return View(db.TriviaTest.ToList());
        }

        // GET: TestsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                RedirectToAction("Index");
            }
            return RedirectToAction("QuestionList", "TestsAdmin", new { Id = id });
        }

        // GET: TestsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestsAdmin/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title")] TriviaTest triviaTest)
        {
            if (ModelState.IsValid)
            {
                triviaTest.Date = DateTime.Now;
                db.TriviaTest.Add(triviaTest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(triviaTest);
        }

        // GET: TestsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaTest triviaTest = db.TriviaTest.Find(id);
            if (triviaTest == null)
            {
                return HttpNotFound();
            }
            return View(triviaTest);
        }

        // POST: TestsAdmin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title")] TriviaTest triviaTest)
        {
            if (ModelState.IsValid)
            {
                triviaTest.Date = DateTime.Now;
                db.Entry(triviaTest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(triviaTest);
        }

        // GET: TestsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaTest triviaTest = db.TriviaTest.Find(id);
            if (triviaTest == null)
            {
                return HttpNotFound();
            }
            return View(triviaTest);
        }

        // POST: TestsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TriviaTest triviaTest = db.TriviaTest.Find(id);
            var triviaQues = db.TriviaQuestions.Where(a => a.TestId == id).ToList();
            foreach(TriviaQuestion item in triviaQues)
            {
                var triviaOpt = db.TriviaOptions.Where(b => b.QuestionId == item.Id).ToList();
                foreach(TriviaOption i in triviaOpt)
                {
                    db.TriviaOptions.Remove(i);
                }
                db.TriviaQuestions.Remove(item);
            }
            db.TriviaTest.Remove(triviaTest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult QuestionList(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.TestId = id;
            var question = db.TriviaQuestions.ToList().Where(a => a.TestId == id);
            return View(question);
        }
        public ActionResult CreateQuestions(int? id)
        {
            if (id == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.Id = id;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuestions([Bind(Include = "Id,Title,TestId")] TriviaQuestion triviaQuestion)
        {
            if (ModelState.IsValid)
            {
                db.TriviaQuestions.Add(triviaQuestion);
                db.SaveChanges();
                return RedirectToAction("CreateOptions", new { id = triviaQuestion.Id });
            }

            return View(triviaQuestion);
        }
        public ActionResult EditQuestions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaQuestion triviaQuest = db.TriviaQuestions.Find(id);
            ViewBag.TestId = triviaQuest.TestId;
            if (triviaQuest == null)
            {
                return HttpNotFound();
            }
            return View(triviaQuest);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestions([Bind(Include = "Id,Title")] TriviaQuestion triviaQuest)
        {
            if (ModelState.IsValid)
            {
                TriviaQuestion tQ = db.TriviaQuestions.Find(triviaQuest.Id);
                tQ.Title = triviaQuest.Title;
                int _id = tQ.TestId;
                db.Entry(tQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuestionList", "TestsAdmin", new { Id = _id });
            }
            return View(triviaQuest);
        }
        public ActionResult DeleteQuestions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriviaQuestion triviaQuest = db.TriviaQuestions.Find(id);
            if (triviaQuest == null)
            {
                return HttpNotFound();
            }
            return View(triviaQuest);
        }
        [HttpPost, ActionName("DeleteQuestions")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuestionsConfirmed(int id)
        {
            TriviaQuestion triviaQuest = db.TriviaQuestions.Find(id);
            var triviaOpt = db.TriviaOptions.Where(b => b.QuestionId == id).ToList();
            foreach (TriviaOption i in triviaOpt)
            {
                db.TriviaOptions.Remove(i);
            }
            int _id = triviaQuest.TestId;
            db.TriviaQuestions.Remove(triviaQuest);
            db.SaveChanges();
            return RedirectToAction("QuestionList", "TestsAdmin", new { Id = _id });
        }


       

        public ActionResult CreateOptions(int id)
        {
            ViewBag.TestId = id;
            return View();
        }


        [HttpPost]

        public ActionResult CreateOptionss()
        {
            int OptionListCount = Request.Form.Count;
            
                if (ModelState.IsValid)
                {
                TriviaOption TriviaOp = new TriviaOption();
                TriviaOp.QuestionId = Int32.Parse(Request.Form[0]);
                TriviaQuestion tq = db.TriviaQuestions.Find(Int32.Parse(Request.Form[0]));
                int _id = tq.TestId;
                int n = 1;

                while (n < OptionListCount)
                {
                    TriviaOp.Title = Request.Form[n];
                    n++;
                    string istrue = "";
                    if (n != OptionListCount)
                    {
                        istrue = Request.Form[n];
                    }                    
                    if (istrue == "on")
                    {
                        TriviaOp.IsCorrect = true;
                        n++;
                    }
                    else
                    {
                        TriviaOp.IsCorrect = false;
                    }
                    db.TriviaOptions.Add(TriviaOp);
                    db.SaveChanges();
                }
                return RedirectToAction("QuestionList", "TestsAdmin", new { Id = _id });
            }
            return View();
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
