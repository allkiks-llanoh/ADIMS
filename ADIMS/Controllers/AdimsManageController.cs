using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class AdimsManageController : Controller
    {
        // GET: AdimsManage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Farmers()
        {
            return View();
        }

        public ActionResult Insurance()
        {
            return View();
        }
        public ActionResult Crops()
        {
            return View();
        }
    }
}