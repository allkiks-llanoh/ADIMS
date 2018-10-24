using ADIMS.Models;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        private readonly adimsEntities db = new adimsEntities();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult CropsAndYield()
        {
            return View();
        }

        [Authorize]
        public ActionResult Insurance()
        {
            return View();
        }

        [Authorize]
        public ActionResult Reports()
        {
            return View();
        }

        [Authorize]
        public ActionResult Farmers()
        {
            return View();
        }

        [Authorize]
        public ActionResult Farms()
        {
            return View();
        }

        [Authorize]
        public ActionResult Crops()
        {
            return View();
        }

    }
}