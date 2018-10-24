using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace ADIMS.Controllers
{
    public class CountiesController : Controller
    {
        // GET: Counties
        private readonly adimsEntities db = new adimsEntities();
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Client, VaryByParam = "id")]
        public JsonResult GetSubCounties(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            int county = Convert.ToInt32(id);

            var subcounties = db.subcounties.OrderBy(x => x.name).Where(x => x.countyid == county).ToList();

            return Json(new SelectList(subcounties, "id", "name"));
        }

        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Client, VaryByParam = "id")]
        public JsonResult GetWards(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            int subcounty = Convert.ToInt32(id);

            var wards = db.wards.OrderBy(x => x.name).Where(x => x.subcounty == subcounty).ToList();

            return Json(new SelectList(wards, "id", "name"));
        }

        public JsonResult GetUAIs(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            int ward = Convert.ToInt32(id);

            var uais = db.uais.OrderBy(x => x.name).Where(x => x.ward == ward).ToList();

            return Json(new SelectList(uais, "id", "name"));
        }

        [HttpGet]
        public ActionResult AddSubCounty()
        {
            ViewBag.counties = db.counties.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult AddWard()
        {
            ViewBag.subcounties = db.subcounties.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddCountyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            county newcounty = new county()
            {
                name = viewmodel.name,
                code = viewmodel.code
            };

            db.counties.Add(newcounty);
            db.SaveChanges();

            return Redirect("/datacapture/counties");
        }

        [HttpPost]
        public ActionResult AddSubCounty(AddSubCountyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            subcounty newsubcounty = new subcounty()
            {
                name = viewmodel.name,
                code = viewmodel.code
            };

            db.subcounties.Add(newsubcounty);
            db.SaveChanges();
            return Redirect("/datacapture/counties");
        }

        [HttpPost]
        public ActionResult AddWard(AddWardViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            ward newward = new ward()
            {
                name = viewmodel.name,
                code = viewmodel.code
            };

            db.wards.Add(newward);
            db.SaveChanges();
            return Redirect("/datacapture/counties");
        }
    }
}