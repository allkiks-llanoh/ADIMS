using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class SeasonsController : Controller
    {
        // GET: Seasons
        private readonly adimsEntities db = new adimsEntities();
        public ActionResult Index()
        {
            return View(db.seasons.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddSeasonViewModel viewmodel)
        {
            season _season = new season()
            {
                name = viewmodel.name,
                startdate = viewmodel.startdate,
                enddate = viewmodel.enddate,
                createdby = 1,
                createdon = DateTime.Now
            };
            db.seasons.Add(_season);
            db.SaveChanges();


            return Redirect("/seasons/index");
        }
    }
}