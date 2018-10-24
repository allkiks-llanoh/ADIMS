using ADIMS.Models;
using ADIMS.Services;
using ADIMS.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ADIMS.APIs
{
    [RoutePrefix("api/cropacreage")]
    public class CropAcreageController : ApiController
    {
        //the database object
        private readonly adimsEntities db = new adimsEntities();

        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add(AddCropAcreageViewModel viewmodel)
        {
            var farm = db.farms.Find(viewmodel.farmid);

            cropacreage cropacreage = new cropacreage()
            {
                farmid = viewmodel.farmid,
                cropid = viewmodel.cropid,
                acreage = viewmodel.acreage,
                season = viewmodel.season,
                year = viewmodel.year,
                variety = viewmodel.variety,

                createdon = DateTime.Now,
                createdby = 1
            };

            var _crop = db.crops.Find(viewmodel.cropid);

            db.cropacreages.Add(cropacreage);

            //Perform Operation
            if (viewmodel.addToQueue && viewmodel.uai != 0)
            {
                var _queue = db.crop_cutting_queue.FirstOrDefault( x =>
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

            string message = $"Dear {farm.farmer.firstname},\n your crop {_crop.name} on {viewmodel.acreage} Ha. has been added successfully. Farm identifier is {farm.name}";
            SMSService.Send(Convert.ToString(farm.farmer.phoneno), message);

            return Ok("Crops Added successfully");
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
        [Route("getcropacreage/{id}")]
        public IHttpActionResult GetCropAcreage(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var _cropAcreage = db.cropacreages
                                .Include(x => x.farm)
                                .Include(x => x.crop)
                                .Include(x => x.cropvariety)
                                .FirstOrDefault(x => x.id == id);

            return Ok(_cropAcreage);
        }


    }
}
