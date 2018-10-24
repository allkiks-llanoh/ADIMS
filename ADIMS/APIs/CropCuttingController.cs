using ADIMS.Models;
using ADIMS.ViewModels;
using AutoMapper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ADIMS.APIs
{
    [RoutePrefix("api/cropcutting")]
    public class CropCuttingController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [HttpPost]
        [Route("preliminary")]
        public IHttpActionResult Preliminary(PreliminaryCropCuttingViewModel viewmodel)
        {
            var _prelimary = Mapper.Map<preliminary_cropcutting_exercise>(viewmodel);

            _prelimary.createdby = 1;
            _prelimary.createdon = DateTime.Now;

            var _ward = db.wards.Find(viewmodel.ward);
            var subcounty = _ward?.subcounty;
            _prelimary.county = db.subcounties.FirstOrDefault(x => x.id == subcounty)?.countyid;
            _prelimary.sub_county = _ward?.subcounty;
            _prelimary.ward = _ward?.id;

            db.preliminary_cropcutting_exercise.Add(_prelimary);

            var item = db.cce_queue_farms.FirstOrDefault(x => x.queue == viewmodel.queue && x.cropacreage == viewmodel.cropacerage);

            if (item != null)
            {
                item.preliminary_status = "Complete";
                item.preliminary_cce = _prelimary.id;

                db.Entry(item).State = EntityState.Modified;
            }

            db.SaveChanges();

            return Ok("Added Successfully");
        }

        [HttpPost]
        [Route("actual")]
        public IHttpActionResult ActualCropCutting(ActualCropCuttingViewModel viewmodel)
        {
            var _actual = Mapper.Map<actual_cropcutting_exercise>(viewmodel);

            _actual.createdby = 1;
            _actual.createdon = DateTime.Now;

            var _ward = db.wards.Find(viewmodel.ward);
            var subcounty = _ward?.subcounty;
            _actual.county = db.subcounties.FirstOrDefault(x => x.id == subcounty)?.countyid;
            _actual.sub_county = _ward?.subcounty;
            _actual.ward = _ward?.id;

            _actual.cropacreage = viewmodel.cropacerage;
            
            db.actual_cropcutting_exercise.Add(_actual);
            
            var item = db.cce_queue_farms.FirstOrDefault(x => x.queue == viewmodel.queue && x.cropacreage == viewmodel.cropacerage);

            if (item != null)
            {
                item.actual_status = "Complete";
                item.actual_cce = _actual.id;

                db.Entry(item).State = EntityState.Modified;
            }


            db.SaveChanges();

            return Ok("Actual CropCutting Added Successfully");
        }
    }
}
