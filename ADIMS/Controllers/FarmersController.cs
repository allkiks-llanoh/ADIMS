using ADIMS.Models;
using ADIMS.ViewModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.IO;
using ADIMS.Extensions;
using ADIMS.Services;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;

namespace ADIMS.Controllers
{
    public class FarmersController : Controller
    {
        private readonly adimsEntities db = new adimsEntities();
        // GET: Farmers
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add(string progresskey = null)
        {
            
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();
            
            ViewBag.counties = counties;
            ViewBag.wards = wards;

            ViewBag.insurancetypes = new List<string>()
            {
                "Weather Index Insurance",
                "Individual indemnity insurance",
                "Area Yield Index Insurance",
                "Any other Crop Insurance"
            };

            ViewBag.insurers = db.insurance_company.Select(x => x.name).ToList();
            ViewBag.crops = db.crops.Select(x => x.name).ToList();

            AddFarmerViewModel _viewmodel;
            if (progresskey == null)
            {
                _viewmodel = new AddFarmerViewModel() { progresskey = Guid.NewGuid().ToString() };
            }
            else
            {
                var item = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == progresskey);
                _viewmodel = JsonConvert.DeserializeObject<AddFarmerViewModel>(item.data);
            }
                
            return View(_viewmodel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult List()
        {
            var farmers = db.farmers.Where(x => x.status != "D").OrderByDescending(x => x.id).Take(200).ToList();
            return View(farmers);
        }

        //Function to check whether another person with same id already exists
        private bool FarmerExists(int idno)
        {
            return db.farmers.Count(x => x.idnumber == idno) > 0 ? true : false;
        }

        [HttpGet]
        public ActionResult ProgressList()
        {
            var _username = User.Identity.Name;

            ViewBag.individuals = db.farmer_ind_progress.Where(x => x.user == _username).ToList();
            ViewBag.corporates = db.farmer_corp_progress.Where(x => x.user == _username).ToList();
            ViewBag.groups = db.farmer_grp_progress.Where(x => x.user == _username).ToList();

            return View();
        }
        

        [HttpPost]
        public ActionResult SaveIndividual(AddFarmerViewModel viewmodel)
        {
            if (ProgressIndExists(viewmodel.progresskey))
            {
                var item = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                item.data = JsonConvert.SerializeObject(viewmodel);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var _savedata = new farmer_ind_progress()
                {
                    name = $"{viewmodel.firstname} {viewmodel.middlename} {viewmodel.lastname}",
                    progresskey = viewmodel.progresskey,
                    user = User.Identity.Name,
                    data = JsonConvert.SerializeObject(viewmodel),
                    date_created = DateTime.Now
                };

                db.farmer_ind_progress.Add(_savedata);
                db.SaveChanges();
            }

            this.AddNotification("Your progress has been saved succesfully", NotificationType.SUCCESS);
            
            return Redirect("/farmers/newfarmingentity");
        }

        [HttpPost]
        public ActionResult SaveCorporate(AddFarmerCorporateViewModel viewmodel)
        {
            if (ProgressCorpExists(viewmodel.progresskey))
            {
                var item = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                item.data = JsonConvert.SerializeObject(viewmodel);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var _savedata = new farmer_corp_progress()
                {
                    name = $"{viewmodel.name}",
                    progresskey = viewmodel.progresskey,
                    user = User.Identity.Name,
                    data = JsonConvert.SerializeObject(viewmodel),
                    date_created = DateTime.Now
                };

                db.farmer_corp_progress.Add(_savedata);
                db.SaveChanges();
            }

            this.AddNotification("Your progress has been saved succesfully", NotificationType.SUCCESS);

            return Redirect("/farmers/newfarmingentity");
        }

        [HttpPost]
        public ActionResult SaveGroup(AddFarmerGroupViewModel viewmodel)
        {
            if (ProgressGrpExists(viewmodel.progresskey))
            {
                var item = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                item.data = JsonConvert.SerializeObject(viewmodel);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var _savedata = new farmer_grp_progress()
                {
                    name = $"{viewmodel.name}",
                    progresskey = viewmodel.progresskey,
                    user = User.Identity.Name,
                    data = JsonConvert.SerializeObject(viewmodel),
                    date_created = DateTime.Now
                };

                db.farmer_grp_progress.Add(_savedata);
                db.SaveChanges();
            }

            this.AddNotification("Your progress has been saved succesfully", NotificationType.SUCCESS);

            return Redirect("/farmers/newfarmingentity");
        }

        public bool ProgressIndExists(string progresskey)
        {
            return db.farmer_ind_progress.Any(x => x.progresskey == progresskey);
        }

        public bool ProgressCorpExists(string progresskey)
        {
            return db.farmer_corp_progress.Any(x => x.progresskey == progresskey);
        }

        public bool ProgressGrpExists(string progresskey)
        {
            return db.farmer_grp_progress.Any(x => x.progresskey == progresskey);
        }


        [HttpPost]
        public ActionResult Add(AddFarmerViewModel newfarmer)
        {
            var county = db.counties.Find(newfarmer.county);
            var ward = db.wards.Find(newfarmer.ward);

            farmer farmer = new farmer()
            {
                firstname = newfarmer.firstname ?? "",
                lastname = newfarmer.lastname ?? "",
                middlename = newfarmer.middlename ?? "",
                name = $"{newfarmer.firstname} {newfarmer.middlename} {newfarmer.lastname}",
                county = newfarmer.county,
                dob = new DateTime(Convert.ToInt32(newfarmer.dob) ,1 , 1),
                gender = newfarmer.gender,
                entity_type = "individual",
                address = newfarmer.address,
                nexofkintype = newfarmer.nexofkintype,
                nextofkinname = newfarmer.nextofkinname,
                krapin = newfarmer.krapin,
                idnumber = newfarmer.idnumber,
                ward = newfarmer.ward,
                maritalstatus = newfarmer.maritalstatus,
                phoneno = newfarmer.phoneno,
                passportno = newfarmer.passportno,
                createdby = 1,
                createdon = DateTime.Now,
                educationLevel = newfarmer.educationLevel,

               
            };

            //check if other farmers exist
            if (FarmerExists(farmer.idnumber.Value))
            {
                #region Reload View
                var counties = db.counties.OrderBy(x => x.name).ToList();
                var wards = db.wards.ToList();

                ViewBag.counties = counties;
                ViewBag.wards = wards;

                ViewBag.insurancetypes = new List<string>()
                {
                    "Weather Index Insurance",
                    "Individual indemnity insurance",
                    "Area Yield Index Insurance",
                    "Any other Crop Insurance"
                };

                ViewBag.insurers = db.insurance_company.Select(x => x.name).ToList();
                ViewBag.crops = db.crops.Select(x => x.name).ToList();

                #endregion

                this.AddNotification($"Sorry,another farmer exists with the same id number {farmer.idnumber}", NotificationType.WARNING);

                return View(newfarmer);
            }

            db.farmers.Add(farmer);

            var _progressItem = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == newfarmer.progresskey);

            if (_progressItem != null)
            {
                db.Entry(_progressItem).State = EntityState.Deleted;
            }

            db.SaveChanges();

            //ADIMS ID
            farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";

            try
            {

                var _file = Request.Files["file"];
                farmer_photo _farmerPhoto = new farmer_photo()
                {
                    createdby = 1,
                    datecreated = DateTime.Now,
                    photoid = UploadFile(_file),
                    farmerid = farmer.id
                };

                db.farmer_photo.Add(_farmerPhoto);

                farmer.photo = _farmerPhoto.id;

                db.Entry(farmer).State = EntityState.Modified;

            }
            catch (Exception)
            {

            }

            //add farmer insurance
            var farmerInsurance = new farmer_insurance()
            {
                farmer = farmer.id,
                crop = db.crops.Find(newfarmer.crop)?.name ?? "",
                insurance_type = newfarmer.insurance_type,
                insurer = newfarmer.insurer,
                other_insurance = newfarmer.other_insurance ?? "",
                policy_number = newfarmer.policy_number ?? "",
            };

            db.farmer_insurance.Add(farmerInsurance);

            db.SaveChanges();

            //Send the Farmer an SMS

            //First Build a Message
            var smsService = new AfricasTalkingSmsService();
            string message = $"Dear {farmer.name}, you have been successfully registered as a farmer with the Ministry of Agriculture, Kenya. Your ADIMS identification number is {farmer.adimsid}";
            //SMSService.Send(Convert.ToString(farmer.phoneno), message);
            smsService.SendSms(Convert.ToString(farmer.phoneno), message);
            //auditing
            AuditService.AddEntry(farmer, User.Identity.Name, $"Farmer id: {farmer.adimsid}");

            this.AddNotification($"Farmer {farmer.firstname} {farmer.lastname} added successfully", NotificationType.SUCCESS);

            return RedirectToAction("List");
        }

        private int UploadFile(HttpPostedFileBase _file)
        {
            string filepath = "";
            string basepath = "/Documents/Photos/";

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

            photo _photo = new photo()
            {
                url = filepath,
                metadat = "farmer photo",
                datecreated = DateTime.Now,
                createdby = 1
            };

            db.photos.Add(_photo);
            db.SaveChanges();

            return _photo.id;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();

            ViewBag.counties = counties;
            ViewBag.wards = wards;

            var farmer = db.farmers.Find(id);

            EditFarmerViewModel editfarmer = new EditFarmerViewModel()
            {
                id = farmer.id,
                firstname = farmer.firstname,
                lastname = farmer.lastname,
                middlename = farmer.middlename,
                county = farmer.county,
                dob = farmer.dob.Value.Year,
                gender = farmer.gender,
                address = farmer.address,
                nexofkintype = farmer.nexofkintype,
                nextofkinname = farmer.nextofkinname,
                krapin = farmer.krapin,
                idnumber = farmer.idnumber,
                ward = farmer.ward,
                maritalstatus = farmer.maritalstatus,
                phoneno = farmer.phoneno,
                passportno = farmer.passportno
            };

            return View(editfarmer);
        }

        [HttpPost]
        public ActionResult Edit(EditFarmerViewModel viewmodel)
        {
            var farmer = db.farmers.Find(viewmodel.id);

            #region Assign Properties
            farmer.firstname = viewmodel.firstname;
            farmer.lastname = viewmodel.lastname;
            farmer.middlename = viewmodel.middlename;

            farmer.name = $"{viewmodel.firstname} {viewmodel.middlename} {viewmodel.lastname}";

            farmer.dob = new DateTime(Convert.ToInt32(viewmodel.dob), 1, 1);
            farmer.gender = viewmodel.gender;
            farmer.address = viewmodel.address;
            farmer.nexofkintype = viewmodel.nexofkintype;
            farmer.nextofkinname = viewmodel.nextofkinname;
            farmer.krapin = viewmodel.krapin;
            farmer.idnumber = viewmodel.idnumber;
            farmer.maritalstatus = viewmodel.maritalstatus;
            farmer.phoneno = viewmodel.phoneno;
            farmer.passportno = viewmodel.passportno;
            #endregion

            db.Entry(farmer).State = EntityState.Modified;
            db.SaveChanges();

            AuditService.EditEntry(farmer, User.Identity.Name, $"Farmer id: {farmer.adimsid}");

            return Redirect($"/farmers/details/{farmer.id}");
        }

        [HttpGet]
        public ActionResult Search()
        {
            var counties = db.counties.Where(x => x.name != null).OrderBy(x => x.name).ToList();
            var crops = db.crops.Select(x => x.name).GroupBy(x => x).Select(x => x.FirstOrDefault()).ToList();

            counties.Insert(0,new county() { id = 0, name = "National" });
            crops.Insert(0, "All");

            ViewBag.crops = crops;
            ViewBag.counties = counties;
            return View();
        }


        [HttpPost]
        public ActionResult Search(SearchViewModel viewmodel)
        {
            List<farmer> farmers = new List<farmer>();

            //order the list of farmers on descending order based on timestamp
            var expression = BuildExpression(viewmodel);
            farmers = db.farmers.Where(expression).OrderByDescending(x => x.createdon).ToList();
            
            ViewBag.results = farmers;

            AuditService.Search(User.Identity.Name, $"Searched for Farmers , parameters: {JsonConvert.SerializeObject(viewmodel)}");

            var counties = db.counties.Where(x => x.name != null).OrderBy(x => x.name).ToList();
            var crops = db.crops.Select(x => x.name).GroupBy(x => x).Select(x => x.FirstOrDefault()).ToList();

            counties.Insert(0, new county() { id = 0, name = "National" });
            crops.Insert(0,"All");

            ViewBag.crops = crops;
            ViewBag.counties = counties;

            return View();
        }

        private int GetSubCountyID(string name)
        {
            return db.subcounties.FirstOrDefault(x => x.name == name).id;
        }

        public Expression<Func<farmer,bool>> BuildExpression(SearchViewModel searchViewModel)
        {
            var predicate = Utility.True<farmer>();

            predicate = Utility.And(predicate, x => x.status != "D");

            if (searchViewModel.adimsid != null)
                predicate = Utility.And(predicate, x => x.adimsid == searchViewModel.adimsid);

            if (searchViewModel.name != null)
                predicate = Utility.And(predicate, x => x.name.Contains(searchViewModel.name));

            if (searchViewModel.idnumber != 0)
                predicate = Utility.And(predicate, x => x.idnumber == searchViewModel.idnumber);

            if (searchViewModel.phone_number != 0)
                predicate = Utility.And(predicate, x => x.phoneno == searchViewModel.phone_number);

            if (searchViewModel.county != 0 && searchViewModel.county != 0)
                predicate = Utility.And(predicate, x => x.county == searchViewModel.county);

            if (searchViewModel.subcounty != 0)
            {
                predicate = Utility.And(predicate, x => x.ward1.subcounty.Value == searchViewModel.subcounty);
            }

            if(searchViewModel.ward != 0)
                predicate = Utility.And(predicate, x => x.ward == searchViewModel.ward);

            if (searchViewModel.enterprise != null && searchViewModel.enterprise != "All")
            {
                predicate = Utility.And(predicate, x => x.farms.SelectMany(c => c.cropacreages.Select(y => y.crop.name.ToLower())).Contains(searchViewModel.enterprise));
            }

            if (searchViewModel.gender != null && searchViewModel.gender != "all")
                predicate = Utility.And(predicate, x => x.gender == searchViewModel.gender);

            if (searchViewModel.scale != null && searchViewModel.scale != "all")
            {
                if(searchViewModel.scale.ToLower() == "large")
                    predicate = Utility.And(predicate, x => x.farms.Any(f => f.area > 50));
                else
                    predicate = Utility.And(predicate, x => x.farms.Any(f => f.area < 50));
            }

            if (searchViewModel.age_range != null && searchViewModel.age_range != "all")
            {
                if (searchViewModel.age_range == "0")
                    predicate = Utility.And(predicate, x => DbFunctions.DiffYears(x.dob, DateTime.Now) <= 36);
                if (searchViewModel.age_range == "1")
                    predicate = Utility.And(predicate, x => DbFunctions.DiffYears(DateTime.Now, x.dob) >= 37);

            }

            return predicate;
        }
        
        public JsonResult SearchFarmer(SearchViewModel viewmodel)
        {
            //db.Configuration.LazyLoadingEnabled = false;
            //db.Configuration.ProxyCreationEnabled = false;

            //List<farmer> farmers = new List<farmer>();
            //if (viewmodel.name != null)
            //    farmers = db.farmers.Include(x => x.farmer_photo).Where(x => x.name.Contains(viewmodel.name)).OrderByDescending(x => x.id).Take(50).ToList();
            //else if (viewmodel.name == null && viewmodel.idnumber > 0)
            //    farmers = db.farmers.Where(x => x.idnumber == viewmodel.idnumber).OrderByDescending(x => x.id).Take(50).ToList();
            //else if (viewmodel.name == null && viewmodel.adimsid != null)
            //    farmers = db.farmers.Where(x => x.adimsid == viewmodel.adimsid).OrderByDescending(x => x.id).Take(50).ToList();
            //else if (viewmodel.name == null && viewmodel.phone_number > 0)
            //    farmers = db.farmers.Where(x => x.phoneno == viewmodel.phone_number).OrderByDescending(x => x.id).Take(50).ToList();

            List<farmer> farmers = new List<farmer>();

            //order the list of farmers on descending order based on timestamp
            var expression = BuildExpression(viewmodel);
            farmers = db.farmers.Where(expression).OrderByDescending(x => x.createdon).ToList();
            return Json(farmers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFarmerFarms(SearchFarmsViewModel viewmodel)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            List<farm> farms = new List<farm>();

            //get the farms
            farms = db.farms.Where(x => x.farmerid == viewmodel.id).OrderByDescending(x => x.id).ToList();

            return Json(farms);
        }

        [HttpPost]
        public JsonResult GetFarmerCropAcreages(SearchFarmsViewModel viewmodel)
        {
            //get the farms
            List<QueryResult> _results = new List<QueryResult>();
            try
            {
                _results = db.farms
                        .Where(x => x.farmerid == viewmodel.id)
                        .SelectMany(x => x.cropacreages.Select(y => new QueryResult()
                        {
                            id = y.id,
                            name = x.name,
                            area = y.acreage,
                            ward = x.ward.name ?? ""
                        }))
                        .ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return Json(_results);
        }

        public class QueryResult
        {
            public int id { get; set; }
            public string name { get; set; }
            public double? area { get; set; }
            public string ward { get; set; }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var farmer = db.farmers.FirstOrDefault(x => x.id == id);

            AuditService.View(User.Identity.Name, $"Viewed details of farmer {farmer.adimsid}");

            return View(farmer);
        }

        [HttpGet]
        public ActionResult DeleteFarmer(int id)
        {
            try
            {
                var farmer = db.farmers.Find(id);

                farmer.status = "D";

                db.Entry(farmer).State = EntityState.Modified;

                db.SaveChanges();

                this.AddNotification("Farmer Deleted", NotificationType.SUCCESS);
            }
            catch (Exception)
            {
                this.AddNotification("Could not complete your delete request. Try again later", NotificationType.ERROR);
            }

           // return View(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult DeleteFarmerFromSearch(int id)
        {
            try
            {
                var farmer = db.farmers.Find(id);

                farmer.status = "D";

                db.Entry(farmer).State = EntityState.Modified;

                db.SaveChanges();

                this.AddNotification("Farmer Deleted", NotificationType.SUCCESS);
            }
            catch (Exception)
            {
                this.AddNotification("Could not complete your delete request. Try again later", NotificationType.ERROR);
            }

            // return View(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Search");
        }

        [HttpGet]
        public ActionResult IdentifyFarmer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IdentifyFarmer(IdentifyFarmerViewModel viewmodel)
        {
            farmer farmer = new farmer();

            switch (viewmodel.identificationoption)
            {
                case IdentificationOption.ID_Number:
                    farmer = db.farmers.FirstOrDefault(x => x.idnumber == viewmodel.id_number);
                    break;
                case IdentificationOption.ADIMS_ID:
                    farmer = db.farmers.FirstOrDefault(x => x.adimsid == viewmodel.adims_id);
                    break;
                case IdentificationOption.PHONE_NUMBER:
                    int phone = Convert.ToInt32(viewmodel.phone_number);
                    farmer = db.farmers.FirstOrDefault(x => x.phoneno == phone);
                    break;
                default:
                    ViewBag.message = "No such Farmer Exists";
                    break;
            }

            ViewBag.farmer = farmer;

            return View();
        }

        [HttpGet]
        public ActionResult NewFarmingEntity()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddFarmerCorporate(string progresskey = null)
        {
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();

            ViewBag.counties = counties;
            ViewBag.wards = wards;

            ViewBag.insurancetypes = new List<string>()
            {
                "Weather Index Insurance",
                "Individual indemnity insurance",
                "Area Yield Index Insurance",
                "Any other Crop Insurance"
            };
            ViewBag.insurers = db.insurance_company.Select(x => x.name).ToList();
            ViewBag.crops = db.crops.Select(x => x.name).ToList();

            AddFarmerCorporateViewModel _viewmodel;
            if (progresskey == null)
            {
                _viewmodel = new AddFarmerCorporateViewModel() { progresskey = Guid.NewGuid().ToString() };
            }
            else
            {
                var item = db.farmer_corp_progress.FirstOrDefault(x => x.progresskey == progresskey);
                _viewmodel = JsonConvert.DeserializeObject<AddFarmerCorporateViewModel>(item.data);
            }

            return View(_viewmodel);
        }

        [HttpGet]
        public ActionResult AddFarmerGroup(string progresskey = null)
        {
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();

            ViewBag.counties = counties;
            ViewBag.wards = wards;

            ViewBag.insurancetypes = new List<string>()
            {
                "Weather Index Insurance",
                "Individual indemnity insurance",
                "Area Yield Index Insurance",
                "Any other Crop Insurance"
            };
            ViewBag.insurers = db.insurance_company.Select(x => x.name).ToList();
            ViewBag.crops = db.crops.Select(x => x.name).ToList();
            
            AddFarmerGroupViewModel _viewmodel;
            if (progresskey == null)
            {
                _viewmodel = new AddFarmerGroupViewModel() { progresskey = Guid.NewGuid().ToString() };
            }
            else
            {
                var item = db.farmer_grp_progress.FirstOrDefault(x => x.progresskey == progresskey);
                _viewmodel = JsonConvert.DeserializeObject<AddFarmerGroupViewModel>(item.data);
            }

            return View(_viewmodel);
        }

        [HttpPost]
        public ActionResult AddFarmerCorporate(AddFarmerCorporateViewModel newfarmer)
        {
            var county = db.counties.Find(newfarmer.county);
            var ward = db.wards.Find(newfarmer.ward);

            farmer farmer = new farmer()
            {
                name = newfarmer.name,
                county = newfarmer.county,
                dob = new DateTime(Convert.ToInt32(newfarmer.date_of_incorporation), 1, 1),
                entity_type = "corporate",
                address = newfarmer.address,
                krapin = newfarmer.kra_pin,
                company_incorporation_number = newfarmer.incorporation_number,
                ward = newfarmer.ward,
                phoneno = newfarmer.phoneno,
                createdby = 1,
                createdon = DateTime.Now
            };

            db.farmers.Add(farmer);

            //ADIMS ID
            farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";

            try
            {
                var _file = Request.Files["file"];

                if (_file != null)
                {
                    farmer_photo _farmerPhoto = new farmer_photo()
                    {
                        createdby = 1,
                        datecreated = DateTime.Now,
                        photoid = UploadFile(_file),
                        farmerid = farmer.id
                    };
                    db.farmer_photo.Add(_farmerPhoto);
                    db.SaveChanges();

                    farmer.photo = _farmerPhoto.id; 
                }
            }
            catch (Exception)
            {

            }

            db.Entry(farmer).State = EntityState.Modified;

            var _progressItem = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == newfarmer.progresskey);

            if (_progressItem != null)
            {
                db.Entry(_progressItem).State = EntityState.Deleted;
            }

            db.SaveChanges();
            this.AddNotification("Your Corporate has been Successfully Registered, Thank you", NotificationType.SUCCESS);

            AuditService.AddEntry(farmer, User.Identity.Name, $"Farmer id: {farmer.adimsid}");

            return View("NewFarmingEntity");
        }

        [HttpPost]
        public ActionResult AddFarmerGroup(AddFarmerGroupViewModel newfarmer)
        {
            var county = db.counties.Find(newfarmer.county);
            var ward = db.wards.Find(newfarmer.ward);

            farmer farmer = new farmer()
            {
                name = newfarmer.name,
                county = newfarmer.county,
                dob = new DateTime(Convert.ToInt32(newfarmer.date_of_creation), 1, 1),
                entity_type = "group",
                address = newfarmer.address,
                group_contact_name = newfarmer.contact_name,
                group_contact_id = newfarmer.contact_id,
                group_first_member = newfarmer.firstmember,
                group_second_member = newfarmer.secondmember,
                group_third_member = newfarmer.thirdmember,
                ward = newfarmer.ward,
                phoneno = newfarmer.phoneno,
                createdby = 1,
                createdon = DateTime.Now
            };

            db.farmers.Add(farmer);

            var _progressItem = db.farmer_ind_progress.FirstOrDefault(x => x.progresskey == newfarmer.progresskey);

            if (_progressItem != null)
            {
                db.Entry(_progressItem).State = EntityState.Deleted;
            }

            db.SaveChanges();

            //ADIMS ID
            farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";

            try
            {
                var _file = Request.Files["file"];

                if (_file != null)
                {
                    farmer_photo _farmerPhoto = new farmer_photo()
                    {
                        createdby = 1,
                        datecreated = DateTime.Now,
                        photoid = UploadFile(_file),
                        farmerid = farmer.id
                    };
                    db.farmer_photo.Add(_farmerPhoto);

                    farmer.photo = _farmerPhoto.id;
                }
            }
            catch (Exception)
            {

            }

            db.Entry(farmer).State = EntityState.Modified;

            db.SaveChanges();

            this.AddNotification("Your Group/Joint Ownership has been Successfully Registered, Thank you", NotificationType.SUCCESS);

            AuditService.AddEntry(farmer, User.Identity.Name, $"Farmer id: {farmer.adimsid}");

            return View("NewFarmingEntity");
        }

       
    }
}