using ADIMS.Models;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class CropProductionReportsController : Controller
    {
        // GET: CropProductionReports
        private readonly adimsEntities db = new adimsEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CountyWeatherDataReport()
        {
            return View(db.county_weather_data.ToList());
        }
        public ActionResult CropProductionStatisticsReport()
        {
            return View(db.crop_production_statistics.ToList());
        }
        public ActionResult DisasterWarningDataReport()
        {
            return View(db.disaster_warning_data.ToList());
        }
        public ActionResult EarlyWarningReport()
        {
            return View(db.early_warning.ToList());
        }
        public ActionResult FoodStocksDataReport()
        {
            return View(db.foodstocks_data.ToList());
        }
        public ActionResult GovtSubsidizedFertilizerReport()
        {
            return View(db.govt_subsidized_fertilizer.ToList());
        }
        public ActionResult InputCostAvailabilityReport()
        {
            return View(db.input_cost_and_availability.ToList());
        }
        public ActionResult PlantingFertilizerReport()
        {
            return View(db.planting_fertilizer.ToList());
        }
        public ActionResult PriceTrendReport()
        {
            return View(db.price_trend.ToList());
        }
        public ActionResult TopDressingFertilizerReport()
        {
            return View(db.top_dressing_fertilizer.ToList());
        }
    }
}