using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Documents
        private readonly adimsEntities db = new adimsEntities();

        [HttpGet]
        public ActionResult All()
        {
            var documents = db.documents.Take(50).ToList();
            return View(documents);
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(FileUploadViewModel viewmodel)
        {
            document newdoc = new document();

            //get last id
            var id = db.documents.OrderByDescending(x => x.id).FirstOrDefault()?.id;
            var newid = id + 1;

            newdoc.id = newid.GetValueOrDefault();
            newdoc.dateadded = DateTime.Now;
            newdoc.description = viewmodel.description;
            newdoc.name = viewmodel.name;
            newdoc.tags = viewmodel.tags;
            var _file = Request.Files["file"];
            newdoc.url = UploadFile(_file);
           

            db.documents.Add(newdoc);
            db.SaveChanges();

            this.AddNotification("File Added Successfully", NotificationType.SUCCESS);

            return RedirectToAction("All");
        }

        private string UploadFile(HttpPostedFileBase _file)
        {
            string filepath = "";
            string basepath = "/Documents/";

            if (_file != null)
            {
                var file = _file;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath(basepath), fileName);
                    file.SaveAs(path);
                    filepath = basepath + fileName;
                }
            }

            return filepath;
        }


    }
}