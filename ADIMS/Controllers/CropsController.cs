using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace ADIMS.Controllers
{
    public class CropsController : Controller
    {
        // GET: Crops
        private readonly adimsEntities db = new adimsEntities();

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.unitsofmeasure = db.unitofmeasures.ToList();
            ViewBag.plantingmethods = db.plantingmethods.ToList();

            return View();
        }

        [OutputCache(Duration = 86400, Location = OutputCacheLocation.Client, VaryByParam = "id")]
        public JsonResult GetVarieties(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            int crop = Convert.ToInt32(id);

            var varieties = db.cropvarieties.Where(x => x.cropid == crop).ToList();

            return Json(new SelectList(varieties, "id", "name"));
        }

        [HttpPost]
        public ActionResult Add(AddCropViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            if (CropExists(viewmodel.name))
            {
                this.AddNotification("Sorry, a crop already exists with the same name", NotificationType.ERROR);
                return View(viewmodel);
            }
            crop newcrop = new crop
            {
                name = viewmodel.name,
                localnames = viewmodel.localnames,
                maturityperiod = viewmodel.maturityperiod,
                plantingmethod = viewmodel.plantingmethod,
                unitmeasure = viewmodel.unitmeasure,
                createdon = DateTime.Now,
            };

            //add to database
            db.crops.Add(newcrop);
            db.SaveChanges();

            this.AddNotification("Crop added successfully", NotificationType.SUCCESS);
            return Redirect("List");
        }

        public bool CropExists(string cropName)
        {
            return db.crops.Any(x => x.name == cropName);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var crop = db.crops.Find(id);

            var viewmodel = new EditCropViewModel()
            {
                id = id,
                name = crop.name,
                localnames = crop.localnames,
                maturityperiod = crop.maturityperiod,
                plantingmethod = crop.plantingmethod,
                unitmeasure = crop.unitmeasure
            };

            ViewBag.unitsofmeasure = db.unitofmeasures.ToList();
            ViewBag.plantingmethods = db.plantingmethods.ToList();

            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Edit(EditCropViewModel model)
        {

            if (!ModelState.IsValid)
                this.AddNotification("There is an error with the data you are posting.", "WARNING");


            try
            {

                db.Configuration.ProxyCreationEnabled = false;

                var result = new crop()
                {
                    id = model.id,
                    name = model.name,
                    localnames = model.localnames,
                    maturityperiod = model.maturityperiod,
                    unitmeasure = model.unitmeasure,
                    plantingmethod = model.plantingmethod
                };

                db.Entry<crop>(result).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                this.AddNotification($"{result.name} crop was edited successfully.", "Success");
            }
            catch (Exception)
            {
                
            }

            return Edit(model.id);

        }

        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var _crop = db.crops.Find(id);

            db.crops.Remove(_crop);
            db.SaveChanges();

            this.AddNotification($"The crop[{_crop.name}] has been successfully deleted", NotificationType.SUCCESS);

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddVariety(bool? fromCropList,int? cropId)
        {
            ViewBag.fromCropList = fromCropList;

            db.Configuration.ProxyCreationEnabled = false;

            if (fromCropList.Value)
            {
                ViewBag.crops = db.crops.Where(x => x.id == cropId).ToList();
            }
            else
            {
                ViewBag.crops = db.crops.ToList();
            }


            return View();
        }

        [HttpPost]
        public ActionResult AddVariety(AddVarietyViewModel viewmodel,bool fromCropList)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            cropvariety newvariety = new cropvariety()
            {
                name = viewmodel.name,
                cropid = viewmodel.cropid
            };

            db.cropvarieties.Add(newvariety);
            db.SaveChanges();

            if (fromCropList)
                return List();
            return Redirect("/datacapture/cropsandyield");
        }

        [HttpGet]
        public ActionResult List()
        {
            return View(db.crops.ToList());
        }
    }
}