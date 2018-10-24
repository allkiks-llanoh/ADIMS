using ADIMS.Models;
using System.Data.Entity;
using System.Web.Http;
using ADIMS.ViewModels;
using System.Linq;
using ADIMS.Services;
using System;
using Newtonsoft.Json;

namespace ADIMS.APIs
{
    [RoutePrefix("api/farms")]
    public class FarmsController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddFarm(AddFarmViewModel newfarm)
        {
            var farmer = db.farmers.Find(newfarm.farmerid);

            farm farm = new farm()
            {
                area = newfarm.area,
                wardid = newfarm.wardid,
                aez = newfarm.aez,
                countyid = newfarm.countyid,
                farmerid = newfarm.farmerid,
                soiltype = newfarm.soiltype,
                topography = newfarm.topography,
                farmingmethod = newfarm.farmingmethod,
                lrnumber = newfarm.lrnumber,
                status = "C",
                createdon = DateTime.Now,
                createdby = 1
            };

            string _wardname = db.wards.Find(newfarm.wardid).name;

            db.farms.Add(farm);

            try
            {
                farmcoordinate newcoordinate = new farmcoordinate()
                {
                    farmid = farm.id,
                    latitude = newfarm.latitude,
                    longitude = newfarm.longitude,
                    altitude = newfarm.altitude
                };

                db.farmcoordinates.Add(newcoordinate);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
            }

            try
            {
                farm.name = $"{farmer.adimsid}/{farm.id}";
                db.Entry(farm).State = EntityState.Modified;

                db.SaveChanges();

                string message = $"Dear {farmer.firstname}, your {farm.area} hectare farm in {_wardname ?? ""} has been successfully registered with the Ministry of Agriculture.";
                SMSService.Send(Convert.ToString(farmer.phoneno), message);

            }
            catch (Exception)
            {
                return Ok("An error occured adding farm");
            }
            return Ok("Successfully added farm");
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetFarm(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var farm = db.farms.Include(x => x.farmcoordinates)
                                .Include(x => x.ward)
                                .Include(x => x.soiltype1)
                                .Include(x => x.farmingmethod1)
                                .Include(x => x.farmer)
                                .Include(x => x.county)
                                .FirstOrDefault(x => x.id == id);

            var serializer = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            return Json(farm, serializer);
        }

        [Route("byward/{ward}")]
        [HttpGet]
        public IHttpActionResult GetFarmsByWard(int ward)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var _farms = db.farms.Include(x => x.ward)
                                 .Include(x => x.soiltype1)
                                 .Include(x => x.farmingmethod1)
                                 .Include(x => x.farmer)
                                 .Include(x => x.county)
                                 .Where(x => x.wardid == ward)
                                 .ToList();

            return Ok(_farms);
        }
    }
}
