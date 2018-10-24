using ADIMS.App_Start;
using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.Services;
using ADIMS.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class CropInsuranceController : Controller
    {
        //the database object
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

        public ActionResult ProgressList()
        {
            var _username = User.Identity.Name;

            ViewBag.preliminary = db.cce_pre_progress.Where(x => x.user == _username).ToList();
            ViewBag.actual = db.cce_actual_progress.Where(x => x.user == _username).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult SavePreliminary(PreliminaryCropCuttingViewModel viewmodel)
        {
            if (ProgressCCEPreExists(viewmodel.progresskey))
            {
                var item = db.cce_pre_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                item.data = JsonConvert.SerializeObject(viewmodel);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var _savedata = new cce_pre_progress()
                {
                    name = $"{viewmodel.respondent_name}",
                    progresskey = viewmodel.progresskey,
                    user = User.Identity.Name,
                    queue = viewmodel.queue,
                    cropacreage = viewmodel.cropacerage,
                    data = JsonConvert.SerializeObject(viewmodel),
                    date_created = DateTime.Now
                };

                db.cce_pre_progress.Add(_savedata);
                db.SaveChanges();
            }

            this.AddNotification("Your progress has been saved succesfully", NotificationType.SUCCESS);

            return Redirect($"/cropinsurance/ccequeuedetails?id={viewmodel.queue}");
        }

        [HttpPost]
        public ActionResult SaveActual(ActualCropCuttingViewModel viewmodel)
        {
            if (ProgressCCEActExists(viewmodel.progresskey))
            {
                var item = db.cce_actual_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                item.data = JsonConvert.SerializeObject(viewmodel);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var _savedata = new cce_actual_progress()
                {
                    name = $"{viewmodel.respondent_name}",
                    progresskey = viewmodel.progresskey,
                    user = User.Identity.Name,
                    queue = viewmodel.queue,
                    cropacreage = viewmodel.cropacerage,
                    data = JsonConvert.SerializeObject(viewmodel),
                    date_created = DateTime.Now
                };

                db.cce_actual_progress.Add(_savedata);
                db.SaveChanges();
            }

            this.AddNotification("Your progress has been saved succesfully", NotificationType.SUCCESS);

            return Redirect($"/cropinsurance/ccequeuedetails?id={viewmodel.queue}");
        }

        public bool ProgressCCEPreExists(string progresskey)
        {
            return db.cce_pre_progress.Any(x => x.progresskey == progresskey);
        }

        public bool ProgressCCEActExists(string progresskey)
        {
            return db.cce_actual_progress.Any(x => x.progresskey == progresskey);
        }


        // GET: CropInsurance
        public ActionResult Preliminary(int cropacreage, int queue, string progresskey = null)
        {
            var _cropacreage = db.cropacreages.Find(cropacreage);
            var farm = _cropacreage.farmid;

            var _farm = db.farms.Find(farm);
            var farmer = _farm.farmer;

            ViewBag.queue = queue;
            ViewBag.cropacreage = cropacreage;

            ViewBag.crops = db.crops.OrderBy(x => x.name).ToList();
            ViewBag.farmer = farmer;
            ViewBag.farm = _farm;
            ViewBag.ward = _farm.ward;
            ViewBag.subcounty = db.subcounties.FirstOrDefault(x => x.id == _farm.ward.subcounty);
            ViewBag.county = _farm.county;

            ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
            ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();

            PreliminaryCropCuttingViewModel _viewmodel;
            if (progresskey == null)
            {
                _viewmodel = new PreliminaryCropCuttingViewModel()
                {
                    crop = _cropacreage.cropid,
                    cropacerage = cropacreage,
                    queue = queue,
                    variety = _cropacreage.cropvariety.name,
                    planting_season = db.seasons.Find(_cropacreage.season)?.name ?? "Long Rains",
                    planted_holding_area = Convert.ToString(_farm?.area ?? 0),
                    progresskey = Guid.NewGuid().ToString()
                };
            }
            else
            {
                var item = db.cce_pre_progress.FirstOrDefault(x => x.progresskey == progresskey);
                _viewmodel = JsonConvert.DeserializeObject<PreliminaryCropCuttingViewModel>(item.data);
            }
            
            return View(_viewmodel);
        }

        public ActionResult CCE(int queue)
        {
            var _queue = db.crop_cutting_queue.Find(queue);

            ViewBag.queue_name = _queue.name;
            ViewBag.queue_id = Convert.ToString(_queue.id);

            return View();
        }

        public ActionResult ActualCropCutting(int cropacreage, int queue, string progresskey = null)
        {
            var farm = db.cropacreages.Find(cropacreage).farmid;

            var _farm = db.farms.Find(farm);
            var farmer = _farm.farmer;

            ViewBag.queue = queue;
            ViewBag.cropacreage = cropacreage;

            ViewBag.farmer = farmer;
            ViewBag.farm = _farm;
            ViewBag.ward = _farm.ward;
            ViewBag.subcounty = db.subcounties.FirstOrDefault(x => x.id == _farm.ward.subcounty);
            ViewBag.county = _farm.county;

            ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
            ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();


            //get the queue info
            var _cce_farm = db.cce_queue_farms.FirstOrDefault(x => x.queue == queue && x.cropacreage == cropacreage);

            
            ActualCropCuttingViewModel _viewmodel;
            if (progresskey == null)
            {
                _viewmodel = new ActualCropCuttingViewModel()
                {
                    enumeration_area_code = _cce_farm?.preliminary_cropcutting_exercise?.enumeration_area_code,
                    village_name = _cce_farm?.preliminary_cropcutting_exercise?.village_name,
                    cropacerage = cropacreage,
                    queue = queue,
                    progresskey = Guid.NewGuid().ToString()
                };
            }
            else
            {
                var item = db.cce_actual_progress.FirstOrDefault(x => x.progresskey == progresskey);
                _viewmodel = JsonConvert.DeserializeObject<ActualCropCuttingViewModel>(item.data);
            }
            return View(_viewmodel);
        }

        [HttpPost]
        public ActionResult Preliminary(int ward, int queue, int cropacreage, PreliminaryCropCuttingViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                this.AddNotification("There is an error in the fields entered.", NotificationType.ERROR);
                return View(viewmodel);
            }

            try
            {
                var _prelimary = Mapper.Map<preliminary_cropcutting_exercise>(viewmodel);
                _prelimary.createdby = 1;
                _prelimary.createdon = DateTime.Now;

                var _ward = db.wards.Find(ward);
                _prelimary.county = db.subcounties.FirstOrDefault(x => x.id == _ward.subcounty)?.countyid;
                _prelimary.sub_county = _ward.subcounty;
                _prelimary.ward = ward;

                db.preliminary_cropcutting_exercise.Add(_prelimary);
                db.SaveChanges();

                this.AddNotification("Preliminary Crop Cutting Successful", NotificationType.SUCCESS);

                //update farm queue status to completed
                var item = db.cce_queue_farms.FirstOrDefault(x => x.queue == queue && x.cropacreage == cropacreage);

                item.preliminary_status = "Complete";
                item.preliminary_cce = _prelimary.id;

                db.Entry(item).State = EntityState.Modified;

                var _progressItem = db.cce_pre_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                if (_progressItem != null)
                {
                    db.Entry(_progressItem).State = EntityState.Deleted;
                }

                try
                {
                    var _file = Request.Files["file"];
                    if (_file != null)
                    {
                        var path = UploadFile(_file);
                        _prelimary.photo_of_crop = path.ToString();

                        db.Entry(_prelimary).State = EntityState.Modified;
                    }
                }
                catch (Exception)
                {

                }

                db.SaveChanges();

                return Redirect($"/cropinsurance/ccequeuedetails?id={queue}");
            }
            catch (Exception)
            {
                var farm = db.cropacreages.Find(cropacreage).farmid;

                var _farm = db.farms.Find(farm);
                var farmer = _farm.farmer;

                ViewBag.queue = queue;
                ViewBag.cropacreage = cropacreage;

                ViewBag.farmer = farmer;
                ViewBag.farm = _farm;
                ViewBag.ward = _farm.ward;
                ViewBag.subcounty = db.subcounties.FirstOrDefault(x => x.id == _farm.ward.subcounty);
                ViewBag.county = _farm.county;

                ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
                ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();
                
                this.AddNotification("An Error occured while submitting data, please try again later", NotificationType.ERROR);

                return View(viewmodel);
            }
        }

        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private string UploadFile(HttpPostedFileBase _file)
        {
            string filepath = "";
            string basepath = @"\Documents\Photos\";

            if (_file != null)
            {
                var file = _file;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath(basepath), fileName);
                    file.SaveAs(path);
                    filepath = basepath + fileName;
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    await file.InputStream.CopyToAsync(memoryStream);
                    //    var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                    //    memoryStream.WriteTo(fileStream);
                    //}
                }
            }

            return filepath;
        }

        [HttpPost]
        public ActionResult ActualCropCutting(int ward, int queue, int cropacreage, ActualCropCuttingViewModel viewmodel, FormCollection fc, HttpPostedFileBase file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewmodel);
                }

                var _actual = Mapper.Map<actual_cropcutting_exercise>(viewmodel);

                _actual.createdby = 1;
                _actual.createdon = DateTime.Now;

                var _ward = db.wards.Find(ward);
                _actual.county = db.subcounties.FirstOrDefault(x => x.id == _ward.subcounty)?.countyid;
                _actual.sub_county = _ward.subcounty;
                _actual.ward = ward;

                _actual.cropacreage = cropacreage;

                if (_actual.moisture_percentagedried > 0)
                {
                    _actual.moisture_reading = _actual.moisture_percentagedried;
                }

                db.actual_cropcutting_exercise.Add(_actual);
                db.SaveChanges();

                this.AddNotification("Actual Crop Cutting Successful", NotificationType.SUCCESS);

                var item = db.cce_queue_farms.FirstOrDefault(x => x.queue == queue && x.cropacreage == cropacreage);

                item.actual_status = "Complete";
                item.actual_cce = _actual.id;

                db.Entry(item).State = EntityState.Modified;
                
                var _progressItem = db.cce_actual_progress.FirstOrDefault(x => x.progresskey == viewmodel.progresskey);
                if (_progressItem != null)
                {
                    db.Entry(_progressItem).State = EntityState.Deleted;
                }

                db.SaveChanges();

                try
                {


                    var _file = Request.Files["file"];
                    if (_file != null)
                    {

                        var path = UploadFile(_file);
                       
                        _actual.photo_ofSS_before_harvesting = path.ToString();

                        db.Entry(_actual).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                }

                return Redirect($"/cropinsurance/ccequeuedetails?id={queue}");
            }
            catch (Exception)
            {
                var farm = db.cropacreages.Find(cropacreage).farmid;

                var _farm = db.farms.Find(farm);
                var farmer = _farm.farmer;

                ViewBag.queue = queue;
                ViewBag.cropacreage = cropacreage;

                ViewBag.farmer = farmer;
                ViewBag.farm = _farm;
                ViewBag.ward = _farm.ward;
                ViewBag.subcounty = db.subcounties.FirstOrDefault(x => x.id == _farm.ward.subcounty);
                ViewBag.county = _farm.county;

                ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
                ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();

                this.AddNotification("An Error occured while submitting data, please try again later", NotificationType.ERROR);

                return View(viewmodel);
            }
        }

        [HttpGet]
        public ActionResult AddInsuranceCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInsuranceCompany(AddInsuranceCompanyViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            insurance_company company = new insurance_company()
            {
                channel_name = viewmodel.channel_name,
                email = viewmodel.email,
                initials = viewmodel.initials,
                name = viewmodel.name,
                phoneno = viewmodel.phoneno,
                physical_address = viewmodel.physical_address,
                postal_address = viewmodel.postal_address,
                krapin = viewmodel.krapin,
                town = viewmodel.town,
                date_created = DateTime.Now,
                distribution_channel = viewmodel.distribution_channel
            };

            db.insurance_company.Add(company);
            db.SaveChanges();
            return Redirect("/cropinsurance/insurancecompanies");
        }

        [HttpGet]
        public ActionResult AddFarmPolicy(int farm)
        {

            var insurers = db.insurance_company.ToList();
            var _farm = db.farms.Find(farm);
            //var crops = db.crops.ToList();
            var crops = _farm.cropacreages.Select(x => x.crop).ToList();

            ViewBag.thefarm = _farm;
            ViewBag.farm = farm;
            ViewBag.insurers = insurers;
            ViewBag.crops = crops;

            AddFarmPolicyViewModel _policyViewModel = new AddFarmPolicyViewModel()
            {
                uai_name = _farm.ward.name
            };

            return View(_policyViewModel);
        }

        [HttpPost]
        public ActionResult AddFarmPolicy(int farm, AddFarmPolicyViewModel viewmodel)
        {
            var insurer = db.insurance_company.Find(viewmodel.insurance_companyid);
            farm_policy farmpolicy = new farm_policy()
            {
                area_insured = viewmodel.area_insured,
                average_yield = viewmodel.average_yield,
                date_created = DateTime.Now,
                uai_name = viewmodel.uai_name,
                dateofpolicysale = viewmodel.dateofpolicysale ?? DateTime.Now,
                insured_yield = viewmodel.insured_yield,
                sum_insured = viewmodel.sum_insured,
                preferred_coverage = viewmodel.preferred_coverage,
                farmid = farm,
                insurance_companyid = viewmodel.insurance_companyid,
                cropinsured = viewmodel.cropinsured,
                premium_rate = viewmodel.premium_rate,
                subsidy_amount = viewmodel.subsidy_amount,
                total_insured = viewmodel.total_insured,
                total_premium = viewmodel.total_premium,
                final_premium = viewmodel.final_premium,
            };

            db.farm_policy.Add(farmpolicy);
            db.SaveChanges();

            //Assign an auto policy Number
            farmpolicy.policy_number = $"{insurer.initials}/EE/KS/15L/M/{farmpolicy.id}";

            db.Entry(farmpolicy).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            string _insuranceCompany = db.insurance_company.Find(viewmodel.insurance_companyid).name;
            var _farmer = db.farms.Find(farm).farmer;

            QueueManager _queueMan = new QueueManager();
            _queueMan.PublishInsurancePolicy(_insuranceCompany, farmpolicy, _farmer);

            this.AddNotification("Crop Insurance Policy Request Created Successfully", NotificationType.SUCCESS);
            return Redirect($"/cropinsurance/policydetails?id={farmpolicy.id}");
        }


        [HttpGet]
        public ActionResult AddPolicyPremium(int policy)
        {
            ViewBag.policy = policy;
            return View();
        }

        [HttpPost]
        public ActionResult AddPolicyPremium(int policy, AddPolicyPremiumViewModel viewmodel)
        {
            farm_policy_premium policypremium = new farm_policy_premium()
            {
                farm_policy_id = policy,
                date_created = DateTime.Now,
                farmer = viewmodel.farmer,
                amount = viewmodel.amount
            };

            db.farm_policy_premium.Add(policypremium);
            db.SaveChanges();
            return Redirect("/cropinsurance/premiums");
        }

        [HttpGet]
        public ActionResult InsuranceCompanies()
        {
            return View(db.insurance_company.ToList());
        }

        [HttpGet]
        public ActionResult Policies()
        {
            return View(db.farm_policy.ToList());
        }

        [HttpGet]
        public ActionResult InsurerPolicies(int company)
        {
            return View("Policies", db.farm_policy.Where(x => x.insurance_companyid == company).ToList());
        }

        [HttpGet]
        public ActionResult Premiums()
        {
            return View(db.farm_policy_premium.ToList());
        }

        [HttpGet]
        public ActionResult PolicyPremiums(int policy)
        {
            return View("Premiums", db.farm_policy_premium.Where(x => x.farm_policy_id == policy).ToList());
        }

        [HttpGet]
        public ActionResult PolicyDetails(int id)
        {
            return View(db.farm_policy.Find(id));
        }

        [HttpGet]
        public ActionResult Bordereaux()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateClaimsBordereaux()
        {
            var insurers = db.insurance_company.ToList();
            ViewBag.insurers = insurers;

            var counties = db.counties.ToList();
            ViewBag.counties = counties;

            var crops = db.crops.ToList();
            ViewBag.crops = crops;
            return View();
        }

        [HttpGet]
        public ActionResult CreateBordereaux()
        {
            var insurers = db.insurance_company.ToList();
            ViewBag.insurers = insurers;

            var counties = db.counties.ToList();
            ViewBag.counties = counties;

            var crops = db.crops.ToList();
            ViewBag.crops = crops;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBordereaux(CreateBordreauxViewModel viewmodel)
        {
            if (!ModelState.IsValid || viewmodel.subcounty == 0 || viewmodel.wardid == 0)
            {
                this.AddNotification("Invalid Parameters, Please fill all values", NotificationType.ERROR);
                return Redirect("/cropinsurance/createbordereaux");
            }

            var bd = GenerateBordereaux();
            ViewBag.bordereaux = bd;

            ViewBag.insurer = db.insurance_company.Find(viewmodel.insurer);
            ViewBag.crop = db.crops.Find(viewmodel.crop);
            ViewBag.year = viewmodel.year;

            ViewBag.county = db.counties.Find(viewmodel.countyid);
            ViewBag.subcounty = db.subcounties.Find(viewmodel.subcounty);
            ViewBag.ward = db.wards.Find(viewmodel.wardid);


            ViewBag.policies = db.farm_policy.ToList();

            return View("Bordereaux");
        }

        [HttpPost]
        public ActionResult CreateClaimsBordereaux(CreateClaimsBordereauxViewModel viewmodel)
        {
            if (!ModelState.IsValid || viewmodel.subcounty == 0 || viewmodel.wardid == 0)
            {
                this.AddNotification("Invalid Parameters, Please fill all values", NotificationType.ERROR);
                return Redirect("/cropinsurance/createbordereaux");
            }

            var bd = GenerateClaimsBordereaux(viewmodel.actual_yield);
            ViewBag.bordereaux = bd;

            ViewBag.insurer = db.insurance_company.Find(viewmodel.insurer);
            ViewBag.crop = db.crops.Find(viewmodel.crop);
            ViewBag.year = viewmodel.year;

            ViewBag.county = db.counties.Find(viewmodel.countyid);
            ViewBag.subcounty = db.subcounties.Find(viewmodel.subcounty);
            ViewBag.ward = db.wards.Find(viewmodel.wardid);

            ViewBag.actual_yield = viewmodel.actual_yield;

            ViewBag.policies = db.farm_policy.ToList();

            return View("ClaimsBordereaux");
        }

        public List<BordereauxItem> GenerateBordereaux()
        {
            List<BordereauxItem> items = new List<BordereauxItem>();
            float averageYield = 3000;
            float unitPrice = 17, subsidy = 40, premiumRate = 2.5f;
            return Enumerable.Range(5, 5).Select(x => new BordereauxItem(averageYield, x * 10, unitPrice, subsidy, premiumRate)).ToList();
        }

        public List<ClaimsBordereauxItem> GenerateClaimsBordereaux(float actualYield)
        {
            List<ClaimsBordereauxItem> items = new List<ClaimsBordereauxItem>();
            float averageYield = 3000;
            float unitPrice = 17, subsidy = 40, premiumRate = 2.5f;
            return Enumerable.Range(5, 5).Select(x => new ClaimsBordereauxItem(averageYield, x * 10, unitPrice, subsidy, premiumRate, actualYield)).ToList();
        }

        public class BordereauxItem
        {
            public BordereauxItem(float _averageYield, float _coverage, float _unitPrice, float _subsidy, float _premiumRate)
            {
                averageYield = _averageYield;
                coverage = _coverage;
                unit_sum = _unitPrice;
                subsidy_rate = _subsidy;
                premium_rate = _premiumRate;
            }
            private float averageYield;
            public float coverage { get; set; }
            public float insured_yield => (coverage / 100) * averageYield;
            public float unit_sum { get; set; }
            public float sum_insured => unit_sum * insured_yield;
            public float premium_rate { get; set; }
            public float premium => (premium_rate / 100) * sum_insured;
            public float subsidy_rate { get; set; }
            public float premium_subsidy => (subsidy_rate / 100) * premium;
            public float share_premium_farmer => 100 - subsidy_rate;
        }

        public class ClaimsBordereauxItem
        {
            public ClaimsBordereauxItem(float _averageYield, float _coverage, float _unitPrice, float _subsidy, float _premiumRate, float _actualYield)
            {
                averageYield = _averageYield;
                actualYield = _actualYield;
                coverage = _coverage;
                unit_sum = _unitPrice;
                subsidy_rate = _subsidy;
                premium_rate = _premiumRate;
            }
            private float averageYield;
            private float actualYield;
            public float coverage { get; set; }
            public float insured_yield => (coverage / 100) * averageYield;
            public float unit_sum { get; set; }
            public float sum_insured => unit_sum * insured_yield;
            public float premium_rate { get; set; }
            public float premium => (premium_rate / 100) * sum_insured;
            public float subsidy_rate { get; set; }
            public float premium_subsidy => (subsidy_rate / 100) * premium;
            public float share_premium_farmer => 100 - subsidy_rate;
            public float shortfall => (insured_yield - actualYield) <= 0 ? 0 : insured_yield - actualYield;
        }

        [HttpGet]
        public ActionResult ClaimRequest(int id)
        {
            var policy = db.farm_policy.Find(id);
            ViewBag.policy = policy;
            return View();
        }

        [HttpPost]
        public ActionResult ClaimRequest(int id, ClaimRequestViewModel viewmodel)
        {
            var policy = db.farm_policy.Find(id);
            ViewBag.policy = policy;

            this.AddNotification("Claim Received, Your Loss assessment will be processed", NotificationType.SUCCESS);
            return View();
        }

        [HttpGet]
        public ActionResult SearchFarmer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchFarmer(SearchViewModel viewmodel)
        {
            List<farmer> farmers = new List<farmer>();
            if (viewmodel.name != null)
                farmers = db.farmers.Where(x => x.name.Contains(viewmodel.name)).Take(50).ToList();
            else if (viewmodel.name == null && viewmodel.idnumber > 0)
                farmers = db.farmers.Where(x => x.idnumber == viewmodel.idnumber).Take(50).ToList();
            else if (viewmodel.name == null && viewmodel.adimsid != null)
                farmers = db.farmers.Where(x => x.adimsid == viewmodel.adimsid).Take(50).ToList();
            else if (viewmodel.name == null && viewmodel.phone_number > 0)
                farmers = db.farmers.Where(x => x.phoneno == viewmodel.phone_number).Take(50).ToList();

            ViewBag.results = farmers;

            return View();
        }

        [HttpGet]
        public ActionResult FarmerPolicies(int id)
        {
            var farmer = db.farmers.Find(id);
            return View(farmer);
        }

        public ActionResult AddToQueue(int farm)
        {
            var queue = db.crop_cutting_queue.FirstOrDefault();

            if (queue.cce_queue_farms.FirstOrDefault(x => x.cropacreage == farm) != null)
            {
                cce_queue_farms _new = new cce_queue_farms()
                {
                    cropacreage = farm,
                    queue = queue.id,
                    preliminary_status = "Pending",
                    actual_status = "Pending"
                };
                db.cce_queue_farms.Add(_new);
                db.SaveChanges();

                this.AddNotification("Farm Successfully Added to Queue", NotificationType.SUCCESS);
            }
            else
            {
                this.AddNotification("Farm is already enumerated in Queue", NotificationType.SUCCESS);
            }

            return Redirect($"/farms/details?id={farm}");
        }

        [HttpGet]
        public async Task<ActionResult> CCEQueue()
        {
            ViewBag.counties = db.counties.OrderBy(x => x.name).ToList();
            ViewBag.seasons = db.seasons.ToList();

            var user = await getUserInfo(User.Identity.Name);

            //filter only queues to the current county
            var cropCuttingQueuesEntities = db.crop_cutting_queue.Where(x => x.county == user.Item2.id && x.isdeleted != true).ToList();

            var result = GetQueuesWithPercentage(cropCuttingQueuesEntities);

            ViewBag.queues = result;

            ViewBag.overall = result?.Average(x => x?.Item2) ?? 0;

            ViewBag.current = new crop_cutting_queue();

            return View();
        }

        [HttpPost]
        public ActionResult CCEQueue(SearchCCEQueueViewModel viewmodel)
        {
            if (viewmodel.year != 0 && viewmodel.year > DateTime.Now.Year)
                this.AddNotification("Sorry that year is in the future. Please revise.", "Warning");

            ViewBag.counties = db.counties.OrderBy(x => x.name).ToList();
            ViewBag.seasons = db.seasons.ToList();

            var cropCuttingQueuesEntities = db.crop_cutting_queue.Where(x => x.isdeleted != true).ToList();

            var result = SearchCCEQueue(viewmodel, cropCuttingQueuesEntities);

            var queues = GetQueuesWithPercentage(result.ToList());

            ViewBag.queues = queues;

            ViewBag.overall = queues.ToList().Count > 0 ? queues.Average(x => x.Item2) : 0;

            return View();
        }

        public async Task<Tuple<string, county, subcounty>> getUserInfo(string name)
        {
            var user = await UserManager.FindByNameAsync(name);
            //get county
            var county = db.counties.Find(user.county);
            //sub-county
            var subCounty = db.subcounties.Find(user.sub_county);

            return Tuple.Create(user.name, county, subCounty);
        }

        [HttpGet]
        public ActionResult CreateQueue()
        {
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var seasons = db.seasons.ToList();

            ViewBag.counties = counties;
            ViewBag.seasons = seasons;
            ViewBag.uais = db.uais.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CreateQueue(CreateQueueViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                var counties = db.counties.OrderBy(x => x.name).ToList();
                var seasons = db.seasons.ToList();

                ViewBag.counties = counties;
                ViewBag.seasons = seasons;
                ViewBag.uais = db.uais.ToList();

                return View(viewmodel);
            }
            crop_cutting_queue queue = new crop_cutting_queue()
            {
                name = viewmodel.name,
                county = viewmodel.county,
                sub_county = viewmodel.sub_county,
                ward = viewmodel.ward,
                uai = viewmodel.uai,
                season = viewmodel.season,
            };

            db.crop_cutting_queue.Add(queue);
            db.SaveChanges();

            this.AddNotification("Queue added successfully", NotificationType.SUCCESS);
            return RedirectToAction("CCEQueue");
        }

        [HttpGet]
        public ActionResult EditQueue(int id)
        {
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var seasons = db.seasons.ToList();

            ViewBag.counties = counties;
            ViewBag.seasons = seasons;
            ViewBag.uais = db.uais.ToList();

            var _queue = db.crop_cutting_queue.Find(id);

            var edit = new EditQueueViewModel()
            {
                id = _queue.id,
                uai = _queue.uai.GetValueOrDefault(),
                season = _queue.season.GetValueOrDefault(),
                year = _queue.year.GetValueOrDefault()
            };

            return View(edit);
        }

        [HttpPost]
        public ActionResult EditQueue(EditQueueViewModel viewModel)
        {
            var _queue = db.crop_cutting_queue.Find(viewModel.id);
            if (!ModelState.IsValid)
            {
                var counties = db.counties.ToList();
                var seasons = db.seasons.ToList();

                ViewBag.counties = counties;
                ViewBag.seasons = seasons;
                ViewBag.uais = db.uais.ToList();

                return View(viewModel);
            }

            _queue.name = viewModel.name;
            _queue.season = viewModel.season;

            db.Entry(_queue).State = EntityState.Modified;

            db.SaveChanges();

            this.AddNotification("Queue edited successfully", NotificationType.SUCCESS);
            return RedirectToAction("CCEQueue");
        }

        [HttpGet]
        public ActionResult DeleteQueue(int id)
        {
            var _queue = db.crop_cutting_queue.Find(id);
            _queue.isdeleted = true;

            db.Entry(_queue).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CCEQueue");
        }

        public ActionResult CCEQueueDetails(int id)
        {
            var queue = db.crop_cutting_queue.Find(id);
            return View(queue);
        }

        public ActionResult Removefromqueue(int queue, int item)
        {
            //find the queue
            var _farm = db.cce_queue_farms.Find(item);

            try
            {
                db.cce_queue_farms.Remove(_farm);
                db.SaveChanges();

                this.AddNotification("Farm removed from queue successfully", NotificationType.SUCCESS);
            }
            catch (Exception)
            {
                this.AddNotification("An error occured removing item from queue", NotificationType.ERROR);
            }
            return Redirect($"/cropinsurance/ccequeuedetails?id={queue}");
        }
        
        public ActionResult CCEFarmResult()
        {
            return View();
        }


        public double GetQueuePercentage(int queue)
        {
            double result = 0;
            List<Tuple<int, int>> _preliminary = new List<Tuple<int, int>>();

            var _queue = db.crop_cutting_queue.Find(queue);
            foreach (var farm in _queue.cce_queue_farms)
            {
                int prem = farm.preliminary_status == "Complete" ? 50 : 0;
                int actual = farm.actual_status == "Complate" ? 50 : 0;
                _preliminary.Add(Tuple.Create(prem, actual));
            }

            result = _preliminary.Select(x => x.Item1 + x.Item2).Average();

            return result;
        }

        public double GetOverallPercentage(int season, int year)
        {
            double result = 0;

            var queues = db.crop_cutting_queue.Where(x => x.season == season && x.year == year).ToList();

            result = queues.Select(x => GetQueuePercentage(x.id)).Average();

            return result;
        }

       

        private IEnumerable<Tuple<crop_cutting_queue, double>> GetQueuesWithPercentage(List<crop_cutting_queue> values)
        {
            var final = new List<Tuple<crop_cutting_queue, double>>();

            foreach (var item in values)
            {
                var tuple = Tuple.Create<crop_cutting_queue, double>(item, GetQueuePercentage(item));
                final.Add(tuple);
            }

            return final;
        }


        public ActionResult CCEQueueReport()
        {
            IList<CCEQueueReportViewModel> _reports = new List<CCEQueueReportViewModel>();

            var entities = db.crop_cutting_queue.ToList();


            foreach (var item in entities)
            {
                var key = item.cce_queue_farms.Where(x => x.preliminary_status == "Complete" && x.actual_status == "Complete").ToList();

                if (key.Count > 0)
                {
                    var report = new CCEQueueReportViewModel()
                    {
                        UAIName = item?.name,
                        farmsSampled = item.cce_queue_farms.Count,
                        averageFarmSize = item.cce_queue_farms.Average(x => x.cropacreage1?.acreage) ?? 0,
                        averageMoistureContent = item.cce_queue_farms.Average(x => x.actual_cropcutting_exercise?.moisture_reading) ?? 0,
                        averageWetWeight = item.cce_queue_farms.Average(x => x.actual_cropcutting_exercise?.weight_of_wetsample) ?? 0,
                        dryWeight = item.cce_queue_farms.Average(x => x.actual_cropcutting_exercise?.total_weight) ?? 0
                    };

                    _reports.Add(report);
                }

            }

            this.AddNotification("This report only shows completed tasks in the queues. For any item to appear here, preliminary crop cutting and actual crop cutting must be performed on them.", "Info");
            return View(_reports);
        }
       
        public ActionResult CropCuttingResults(int cce_farm)
        {
            var _cce_Farm = db.cce_queue_farms.Find(cce_farm);

            return View(_cce_Farm);
        }

        private double GetQueuePercentage(crop_cutting_queue queue)
        {
            IList<int> values = new List<int>();

            foreach (var item in queue.cce_queue_farms)
            {
                int tentative = 0;

                if (item.preliminary_status == "Complete")
                    tentative += 50;

                if (item.actual_status == "Complete")
                    tentative += 50;


                values.Add(tentative);
            }

            if (values.Count > 0)
                return values.Average();
            else
                return 0;
        }

        internal IEnumerable<crop_cutting_queue> SearchCCEQueue(SearchCCEQueueViewModel model, IEnumerable<crop_cutting_queue> initial)
        {
            //county filter
            if (model.county != 0)
                initial = initial.Where(x => x.county.Value == model.county);

            if (model.subcounty != 0)
                initial = initial.Where(x => x.sub_county.Value == model.subcounty);

            //ward filter
            if (model.ward != 0)
                initial = initial.Where(x => x.ward.Value == model.ward);

            //season filter
            if (model.season != 0)
                initial = initial.Where(x => x.season.Value == model.season);
            
            //year filter
            if (model.year != 0 && model.year <= DateTime.Now.Year)
                initial = initial.Where(x => x.year == model.year);

            return initial.ToList();
        }

       
    }
}