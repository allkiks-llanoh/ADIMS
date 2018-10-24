using ADIMS.Models;
using ADIMS.ViewModels.AYII;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class AYIIController : Controller
    {
        private readonly adimsEntities db = new adimsEntities();

        // GET: AYII
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddUAI()
        {
            ViewBag.counties = db.counties.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddUAI(AddUAIViewModel viewmodel)
        {
            uai _uai = new uai()
            {
                name = viewmodel.name,
                code = viewmodel.code,
                ward = viewmodel.wardid,
                createdby = 1,
                datecreated = DateTime.Now
            };

            db.uais.Add(_uai);
            db.SaveChanges();

            return RedirectToAction("ListUAI");
        }

        [HttpGet]
        public ActionResult ListUAI()
        {
            return View(db.uais.ToList());
        }

        [HttpGet]
        public ActionResult InsuredFarmers()
        {
            var policies = db.ayii_policy.ToList();
            return View(policies);
        }

        [HttpGet]
        public ActionResult UpdateUAIVariables(int uai)
        {
            var _uai = db.uais.Find(uai);

            ViewBag.seasons = db.seasons.ToList();



            ViewBag.uai = _uai;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateUAIVariables(int uai, UpdateUAIVariablesViewModel viewmodel)
        {
            uai_variables _variables = new uai_variables()
            {
                season = viewmodel.season,
                year = viewmodel.year,
                uai = uai,
                historical_average = viewmodel.historical_average,
                average_yield = viewmodel.average_yield,
                coverage = viewmodel.coverage,
                unitvalue = viewmodel.unit_value,
                datemodified = DateTime.Now
            };

            db.uai_variables.Add(_variables);
            db.SaveChanges();

            return Redirect($"/ayii/uaidetails?id={uai}");
        }

        [HttpGet]
        public ActionResult UAIDetails(int id)
        {
            var _uai = db.uais.Find(id);

            var subcounty = db.subcounties.Find(_uai.ward1.subcounty);
            var county = subcounty.county;

            ViewBag.subcounty = subcounty.name;
            ViewBag.county = county.name;

            return View(_uai);
        }

        public ActionResult InsuredFarms(int uai, int season)
        {
            var policies = db.ayii_policy.Where(x => x.uai == uai && x.season == season)
                                         .ToList();

            return View(policies);
        }

        public ActionResult UAIHistory(int uai)
        {
            return View(db.uai_variables.Where(x => x.uai == uai).ToList());
        }

    }
}