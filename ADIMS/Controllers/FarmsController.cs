using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.Services;
using ADIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class FarmsController : Controller
    {
        // GET: Farms
        private readonly adimsEntities db = new adimsEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add(int farmer)
        {
            ViewBag.farmerid = farmer;
            var farmerobj = db.farmers.Find(farmer);

            ViewBag.farmer = farmerobj;

            var soiltypes = db.soiltypes.ToList();
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();
            var farmingmethods = db.farmingmethods.ToList();

            ViewBag.counties = counties;
            ViewBag.wards = wards;
            ViewBag.soiltypes = soiltypes;
            ViewBag.farmingmethods = farmingmethods;
            return View();
        }

        [HttpPost]
        public ActionResult Add(int _farmer, AddFarmViewModel newfarm)
        {
            var farmerid = _farmer;
            var farmer = db.farmers.Find(farmerid);

            var ward = db.wards.Find(newfarm.wardid);

            farm farm = new farm()
            {
                area = newfarm.area,
                countyid = newfarm.countyid,
                wardid = newfarm.wardid,
                farmerid = farmerid,
                soiltype = newfarm.soiltype,
                topography = newfarm.topography,
                farmingmethod = newfarm.farmingmethod,
                lrnumber = newfarm.lrnumber,
                aez = newfarm.aez,
                status = "C",
                createdon = DateTime.Now,
                createdby = 1,
                irrigationMethod = newfarm.irrigationMethod
            };

            db.farms.Add(farm);
            db.SaveChanges();

            farm.name = $"{farmer.adimsid}/{farm.id}";
            db.Entry(farm).State = EntityState.Modified;
            
            farmcoordinate newcoordinate = new farmcoordinate()
            {
                farmid = farm.id,
                latitude = newfarm.latitude,
                longitude = newfarm.longitude,
                altitude = newfarm.altitude
            };

            db.farmcoordinates.Add(newcoordinate);
            db.SaveChanges();
            //First Build a Message
            var smsService = new AfricasTalkingSmsService();
           
            try
            {
                string message = $"Dear {farmer?.firstname}, your {farm?.area} hectare farm in {ward?.name} has been successfully registered with the Ministry of Agriculture.";
               // SMSService.Send(Convert.ToString(farmer.phoneno), message);
                smsService.SendSms(Convert.ToString(farmer.phoneno), message);
            }
            catch (Exception)
            {
                
            }
            
            this.AddNotification($"Farm {farm.name} added successfully", NotificationType.SUCCESS);

            return Redirect($"/farmers/details/{farmerid}");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var _farm = db.farms.Find(id);
            ViewBag.farmerid = _farm.farmerid;
            var farmerobj = _farm.farmer;

            ViewBag.farmer = farmerobj;

            var soiltypes = db.soiltypes.ToList();
            var counties = db.counties.OrderBy(x => x.name).ToList();
            var wards = db.wards.ToList();
            var farmingmethods = db.farmingmethods.ToList();

            ViewBag.counties = counties;
            ViewBag.wards = wards;
            ViewBag.soiltypes = soiltypes;
            ViewBag.farmingmethods = farmingmethods;

            AddFarmViewModel viewmodel = new AddFarmViewModel()
            {
                altitude = _farm.farmcoordinates.FirstOrDefault()?.altitude,
                area = _farm.area,
                countyid = _farm.countyid,
                farmerid = _farm.farmerid,
                farmingmethod = _farm.farmingmethod,
                latitude = _farm.farmcoordinates.FirstOrDefault()?.latitude,
                longitude = _farm.farmcoordinates.FirstOrDefault()?.longitude,
                lrnumber = _farm.lrnumber,
                name = _farm.name,
                soiltype = _farm.soiltype,
                topography = _farm.topography,
                wardid = _farm.wardid
            };

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Edit(int id, AddFarmViewModel newfarm)
        {

            var _farmobj = db.farms.Find(id);
            var farmerid = _farmobj.farmerid;
            var farmer = db.farmers.Find(farmerid);


            _farmobj.area = newfarm.area;
            _farmobj.countyid = newfarm.countyid;
            _farmobj.farmerid = farmerid;
            _farmobj.soiltype = newfarm.soiltype;
            _farmobj.topography = newfarm.topography;
            _farmobj.farmingmethod = newfarm.farmingmethod;
            _farmobj.lrnumber = newfarm.lrnumber;

            db.Entry(_farmobj).State = EntityState.Modified;
           
            db.SaveChanges();

            //find coordinates
            var _coords = _farmobj.farmcoordinates.FirstOrDefault();

            if (_coords != null)
            {
                _coords.latitude = newfarm.latitude;
                _coords.longitude = newfarm.longitude;
                _coords.altitude = newfarm.altitude;


                db.Entry(_coords).State = EntityState.Modified;
                db.SaveChanges();
            }
           

            return Redirect($"/farmers/details/?id={farmerid}");
        }

        [HttpGet]
        public ActionResult AddCrops(int farm)
        {
            ViewBag.farmid = farm;
            var farmobj = db.farms.Find(farm);
            var farmer = farmobj.farmer;

            ViewBag.farmer = farmer;

            var crops = db.crops.OrderBy(x => x.name).ToList();
            var varieties = db.cropvarieties.ToList();

            ViewBag.crops = crops;
            ViewBag.cropvarieties = varieties;

            ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
            ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();

            ViewBag.seasons = db.seasons.ToList();

            var uais = farmobj.ward?.uais?.ToList() ?? new List<uai>();
            ViewBag.uais = uais;

            return View();
        }

        [HttpPost]
        public ActionResult AddCrops(int _farmid, AddCropAcreageViewModel viewmodel)
        {
            var farmid = _farmid;

            var farm = db.farms.Find(farmid);
            double total_crop_acreage = 0;

            if (viewmodel.acreage == 0 || viewmodel.acreage == null)
            {
               
            }

            var total_acreage = farm.area;
            if (farm.cropacreages.Count > 0)
            {
                total_crop_acreage = farm.cropacreages.Sum(x => x.acreage.GetValueOrDefault());
            }
            //check whether what we're adding doesn't exceed total acreage
            if ((viewmodel.acreage + total_crop_acreage) > total_acreage)
            {
                this.AddNotification($"The Crop acreage you're adding exceeds the total farm acreage of {total_acreage}",NotificationType.ERROR);
              
            }
            else
            {
                cropacreage cropacreage = new cropacreage()
                {
                    acreage = viewmodel.acreage,
                    cropid = viewmodel.cropid,
                    farmid = farmid,
                    season = viewmodel.season,
                    variety = viewmodel.variety,
                    year = viewmodel.year,
                    createdby = 1,
                    createdon = DateTime.Now
                };

                db.cropacreages.Add(cropacreage);

                //Add the farm to the queue automatically
                if (viewmodel.addToQueue && viewmodel.uai != 0)
                {
                    var _queue = db.crop_cutting_queue.FirstOrDefault(x => 
                                                    x.uai.Value == viewmodel.uai 
                                                    && x.season == viewmodel.season
                                                    && x.year == viewmodel.year
                                                    );

                    if (_queue == null)
                    {
                        //create the queue
                        var uai = db.uais.Find(viewmodel.uai);
                        var season = db.seasons.Find(viewmodel.season);
                        _queue = CreateQueue(uai, season, viewmodel.year);
                    }

                    cce_queue_farms _cqf = new cce_queue_farms()
                    {
                        queue = _queue.id,
                        cropacreage = cropacreage.id,
                        datecreated = DateTime.Now,
                        createdby = 1
                    };

                    db.cce_queue_farms.Add(_cqf);

                }
                
                if (viewmodel.subscribeAyii)
                {
                    ayii_policy _policy = new ayii_policy()
                    {
                        farmer = farm.farmerid,
                        cropacreage = cropacreage.id,
                        season = viewmodel.season,
                        year = viewmodel.year,
                        uai = viewmodel.uai == 0 ? 1 : viewmodel.uai,
                        datecreated = DateTime.Now
                    };
                    db.ayii_policy.Add(_policy);
                }

                db.SaveChanges();

                var _crop = db.crops.Find(viewmodel.cropid);
                //First Build a Message
                var smsService = new AfricasTalkingSmsService();
             
                try
                {
                    string message = $"Dear {farm.farmer.firstname},\n your crop {_crop.name} on {viewmodel.acreage} Ha. has been added successfully. Farm identifier is {farm.name}";
                   // SMSService.Send(Convert.ToString(farm.farmer.phoneno), message);
                    smsService.SendSms(Convert.ToString(farm.farmer.phoneno), message);
                }
                catch (Exception)
                {
                    
                }

                AuditService.AddEntry(cropacreage, User.Identity.Name, $"Added crop {viewmodel.cropid} on farm {farm.name}");

                return Redirect($"/farms/details/{farmid}");
            }

            #region
            ViewBag.farmid = farm.id;
            var farmobj = db.farms.Find(farm.id);
            var farmer = farmobj.farmer;

            ViewBag.farmer = farmer;

            var crops = db.crops.ToList();
            var varieties = db.cropvarieties.ToList();

            ViewBag.crops = crops;
            ViewBag.cropvarieties = varieties;

            ViewBag.topDressingFerts = db.fertilizers.Where(x => x.application_method == "TopDressing").ToList();
            ViewBag.plantingFerts = db.fertilizers.Where(x => x.application_method == "Planting").ToList();

            ViewBag.seasons = db.seasons.ToList();

            var uais = farmobj.ward.uais.ToList();
            ViewBag.uais = uais;
            #endregion

            return View(viewmodel);
            
        }

        public crop_cutting_queue CreateQueue(uai uai, season season, int year)
        {
            var ward = uai.ward1;
            var subcounty = db.subcounties.Find(ward.subcounty);
            var county = subcounty.county;

            crop_cutting_queue queue = new crop_cutting_queue()
            {
                name = $"{uai.name} {season.name} {year}",
                uai = uai.id,
                county = county.id,
                sub_county = subcounty.id,
                ward = ward.id,
                season = season.id,
                year = year,
                datecreated = DateTime.Now,
                createdby = 1,
                isdeleted = false
            };

            db.crop_cutting_queue.Add(queue);
            db.SaveChanges();

            return queue;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var farm = db.farms.Include(x => x.cropacreages.Select( y => y.cropvariety)).Include(x => x.farmcoordinates).FirstOrDefault(x => x.id == id);

            ViewBag.subcounty = db.subcounties.Find(farm.ward?.subcounty)?.name ?? "";

            AuditService.View(User.Identity.Name, $"Viewed Farm Details {farm.name ?? ""}");
            return View(farm);
        }
    }
}