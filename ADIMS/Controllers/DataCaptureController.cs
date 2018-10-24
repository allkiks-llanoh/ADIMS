using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class DataCaptureController : Controller
    {
        // GET: DataCapture
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
        public ActionResult Counties()
        {
            return View();
        }
    }
}