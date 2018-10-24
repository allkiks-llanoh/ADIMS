using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly adimsEntities db = new adimsEntities();
        private ApplicationDbContext _context = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Settings()
        {
            var subsidy = db.settings.FirstOrDefault(x => x.name == "subsidy").value;
            var unitvalue = db.settings.FirstOrDefault(x => x.name == "unitvalue").value;
            var coverage = db.settings.FirstOrDefault(x => x.name == "coverage").value;

            ViewBag.subsidy = subsidy;
            ViewBag.unitvalue = unitvalue;
            ViewBag.coverage = coverage;

            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AuditLogs()
        {
            return View(db.auditlogs.AsNoTracking().OrderByDescending(x => x.id).Take(100).ToList());
        }

        [HttpPost]
        public ActionResult UpdateSubsidy(string subsidy)
        {
            var item = db.settings.FirstOrDefault(x => x.name == "subsidy");

            item.value = subsidy;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Subsidy Updated Successfully", NotificationType.SUCCESS);
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult UnitValue(string unitvalue)
        {
            var item = db.settings.FirstOrDefault(x => x.name == "unitvalue");

            item.value = unitvalue;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Unit Sum Insured Value Updated Successfully", NotificationType.SUCCESS);
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult Coverage(string coverage)
        {
            var item = db.settings.FirstOrDefault(x => x.name == "coverage");

            item.value = coverage;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            this.AddNotification("Coverage Value Updated Successfully", NotificationType.SUCCESS);
            return RedirectToAction("Settings");
        }

        [HttpGet]
        public ActionResult Users()
        {
            ViewBag.roles = _context.Roles.ToList();

            return View(_context.Users.ToList());
        }
        
        
        [HttpGet]
        public ActionResult Fertilizers()
        {
            return View(db.fertilizers.ToList());
        }

        [HttpGet]
        public ActionResult AddFertilizer()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditFertilizer(int? id)
        {
            if(id != null)
            {
                var fertilizer = db.fertilizers.Find(id);

                var result = new AddFertilizerViewModel()
                {
                    name = fertilizer.name,
                    application_method = fertilizer.application_method
                };

                return View(result);
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditFertilizer(int id, AddFertilizerViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                fertilizer _theFertilizer = new fertilizer()
                {
                    id = id,
                    name = viewmodel.name,
                    application_method = viewmodel.application_method,
                    dateadded = DateTime.Now
                };

                db.Entry(_theFertilizer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.AddNotification($"Fertilizer {id} edited successfully", NotificationType.SUCCESS);

                return Redirect("/admin/fertilizers");
            }
            else
            {
                this.AddNotification("An error occured editing the fertilzer, please try again", NotificationType.ERROR);
                return View(viewmodel);
            }

        }

        public ActionResult DeleteFertilizer(int? id)
        {
            if(id != null)
            {
                var fertilizer = db.fertilizers.Find(id);

                db.fertilizers.Remove(fertilizer);
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult AddFertilizer(AddFertilizerViewModel viewmodel)
        {
            fertilizer _newFertilizer = new fertilizer()
            {
                name = viewmodel.name,
                application_method = viewmodel.application_method,
                dateadded = DateTime.Now
            };
            db.fertilizers.Add(_newFertilizer);
            db.SaveChanges();

            this.AddNotification("Fertilizer added successfully", NotificationType.SUCCESS);

            return RedirectToAction("Fertilizers");
        }

        [HttpGet]
        public ActionResult UaiYield()
        {
            return View(db.uai_yield.ToList());
        }

        [HttpGet]
        public ActionResult AddUAIYield()
        {
            var crops = db.crops.ToList();
            var counties = db.counties.ToList();

            ViewBag.counties = counties;
            ViewBag.crops = crops;

            return View();
        }

        [HttpPost]
        public ActionResult AddUAIYield(AddUAIYield viewmodel)
        {
            if(!ModelState.IsValid)
            {
                this.AddNotification("Please fill all fields.", "ERROR");
            }

            uai_yield _yield = new uai_yield()
            {
                crop = viewmodel.crop,
                ward = viewmodel.ward,
                average_yield = viewmodel.average_yield
            };

            db.uai_yield.Add(_yield);
            db.SaveChanges();

            this.AddNotification("Succesfully added record", NotificationType.SUCCESS);

            return RedirectToAction("UaiYield");
        }
    }
}