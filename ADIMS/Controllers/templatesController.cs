using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class TemplatesController : Controller
    {
        // GET: Templates
        [HttpGet]
        public ActionResult login()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult register()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult forgotpassword()
        {
            return PartialView();
        }
    }
}