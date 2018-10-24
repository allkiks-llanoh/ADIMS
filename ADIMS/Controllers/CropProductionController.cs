using ADIMS.App_Start;
using ADIMS.Models;
using ADIMS.ViewModels.DataCapture_CropProduction;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    [Authorize]
    public class CropProductionController : Controller
    {
        // GET: CropProduction
        private readonly adimsEntities db = new adimsEntities();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddCountyWeatherData()
        {
            var result = await getUserInfo(User.Identity.Name);

            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddCountyWeatherDataViewModel() { county = result.Item2.id, sub_county = result.Item3.id});
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCountyWeatherData(AddCountyWeatherDataViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _weather = Mapper.Map<county_weather_data>(viewmodel);

            db.county_weather_data.Add(_weather);
            db.SaveChanges();

            return RedirectToAction("AddCountyWeatherData");
        }

        [HttpGet]
        public async Task<ActionResult> AddCropProductionStatistics()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddCropProductionStatisticsViewModel() { county = result.Item2.id, sub_county = result.Item3.id });
        }

        [HttpPost]
        public ActionResult AddCropProductionStatistics(AddCropProductionStatisticsViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _cropProduction = Mapper.Map<crop_production_statistics>(viewmodel);

            db.crop_production_statistics.Add(_cropProduction);
            db.SaveChanges();
            return RedirectToAction("AddCropProductionStatistics");
        }

        [HttpGet]
        public async Task<ActionResult> AddDisasterWarningData()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddDisasterWarningDataViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddDisasterWarningData(AddDisasterWarningDataViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _disaster = Mapper.Map<disaster_warning_data>(viewmodel);

            db.disaster_warning_data.Add(_disaster);
            db.SaveChanges();
            return RedirectToAction("AddDisasterWarningData");
        }

        [HttpGet]
        public async Task<ActionResult> AddEarlyWarning()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;
            ViewBag.crops = db.crops.ToList();

            return View(new AddEarlyWarningViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddEarlyWarning(AddEarlyWarningViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _earlyWarning = Mapper.Map<early_warning>(viewmodel);
            db.early_warning.Add(_earlyWarning);
            db.SaveChanges();
            return RedirectToAction("AddEarlyWarning");
        }
        

        [HttpGet]
        public async Task<ActionResult> AddFoodStocksData()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddFoodStocksDataViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddFoodStocksData(AddFoodStocksDataViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _foodstocks = Mapper.Map<foodstocks_data>(viewmodel);

            db.foodstocks_data.Add(_foodstocks);
            db.SaveChanges();
            return RedirectToAction("AddFoodStocksData");
        }

        [HttpGet]
        public async Task<ActionResult> AddGovtSubsidizedFertilizer()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddGovtSubsidizedFertlizerViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddGovtSubsidizedFertilizer(AddGovtSubsidizedFertlizerViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _govtSubsidized = Mapper.Map<govt_subsidized_fertilizer>(viewmodel);

            db.govt_subsidized_fertilizer.Add(_govtSubsidized);
            db.SaveChanges();
            return RedirectToAction("AddGovtSubsidizedFertilizer");
        }

        [HttpGet]
        public async Task<ActionResult> AddInputCostAvailability()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddInputCostAndAvailabilityViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddInputCostAvailability(AddInputCostAndAvailabilityViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _inputCost = Mapper.Map<input_cost_and_availability>(viewmodel);

            db.input_cost_and_availability.Add(_inputCost);
            db.SaveChanges();
            return RedirectToAction("AddInputCostAvailability");
        }

        [HttpGet]
        public async Task<ActionResult> AddPlantingFertilizer()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddPlantingFertilizerViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddPlantingFertilizer(AddPlantingFertilizerViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _plantingFert = Mapper.Map<planting_fertilizer>(viewmodel);

            db.planting_fertilizer.Add(_plantingFert);
            db.SaveChanges();
            return RedirectToAction("AddPlantingFertilizer");
        }

        [HttpGet]
        public async Task<ActionResult> AddTopDressingFertilizer()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddTopDressingFertilizerViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddTopDressingFertilizer(AddTopDressingFertilizerViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _topDressing = Mapper.Map<top_dressing_fertilizer>(viewmodel);

            db.top_dressing_fertilizer.Add(_topDressing);
            db.SaveChanges();
            return RedirectToAction("AddTopDressingFertilizer");
        }

        [HttpGet]
        public async Task<ActionResult> AddPriceTrend()
        {
            var result = await getUserInfo(User.Identity.Name);
            ViewBag.seasons = db.seasons.ToList();
            ViewBag.userInfo = result;

            return View(new AddPriceTrendViewModel() { county = result.Item2.id, sub_county = result.Item3.id });

        }

        [HttpPost]
        public ActionResult AddPriceTrend(AddPriceTrendViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            var _priceTrend = Mapper.Map<price_trend>(viewmodel);

            db.price_trend.Add(_priceTrend);
            db.SaveChanges();
            return RedirectToAction("AddPriceTrend");
        }
        public async Task<Tuple<string, county, subcounty>> getUserInfo(string name)
        {
            var user = await UserManager.FindByNameAsync(name);
            //get county
            var county = db.counties.Find(user.county) ?? db.counties.Find(1);
            //sub-county
            var subCounty = db.subcounties.Find(user.sub_county) ?? db.subcounties.Find(1);

            return Tuple.Create(user.name, county, subCounty);
        }

    }
}