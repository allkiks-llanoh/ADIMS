using ADIMS.Models;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using ADIMS.ViewModels;
using System;
using ADIMS.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ADIMS.APIs
{
    [RoutePrefix("api/farmers")]
    public class FarmersController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();
        private int LIMIT = 50;

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetFarmers()
        {
            db.Configuration.LazyLoadingEnabled = false;
            
            var farmers = db.farmers
                            .Where(x => x.status != "D")
                            .Include(x => x.farmer_photo.Select(p => p.photo))
                            .Include(x => x.ward1)
                            .OrderByDescending(x => x.id)
                            .Take(LIMIT)
                            .ToList();
            var serializer = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            return Json(farmers, serializer);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetFarmer(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var farmer = db.farmers.Include(x => x.county1)
                                   .Include(x => x.ward1)
                                   .FirstOrDefault(x => x.id == id);

            return Ok(farmer);
        }

        [Route("{id}/farms")]
        [HttpGet]
        public IHttpActionResult GetFarmerFarms(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var farms = db.farms
                            .Include(x => x.farmcoordinates)
                            .Include(x => x.ward)
                            .Include(x => x.soiltype1)
                            .Include(x => x.farmingmethod1)
                            .Include(x => x.farmer)
                            .Include(x => x.county)
                            .Where(x => x.farmerid == id)
                            .ToList();

            var serializer = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
            
            return Json(farms, serializer);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddFarmer(AddFarmerViewModel newfarmer)
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
                dob = new DateTime(Convert.ToInt32(newfarmer.dob), 1, 1),
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
                createdon = DateTime.Now
            };

            try
            {
                db.farmers.Add(farmer);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
            }

            try
            {
                farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";

                db.Entry(farmer).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                
            }

            string message = $"Dear {farmer.firstname}\n, you have been successfully registered as a farmer with the Ministry of Agriculture, Kenya. Your ADIMS identification number is {farmer.adimsid}";
            SMSService.Send(Convert.ToString(farmer.phoneno), message);

            return Ok("Successfully added farmer");
        }

        [HttpPost]
        [Route("search")]
        public IHttpActionResult SearchFarmer(SearchViewModel viewmodel)
        {
           
            List<farmer> farmers = new List<farmer>();

            //order the list of farmers on descending order based on timestamp
            var expression = BuildExpression(viewmodel);
            farmers = db.farmers.Where(expression).OrderByDescending(x => x.createdon).ToList();

            var serializer = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            return Json(farmers.OrderByDescending(x => x.createdon), serializer);
        }

        public Expression<Func<farmer, bool>> BuildExpression(SearchViewModel searchViewModel)
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

            if (searchViewModel.ward != 0)
                predicate = Utility.And(predicate, x => x.ward == searchViewModel.ward);

            if (searchViewModel.enterprise != null && searchViewModel.enterprise != "All")
            {
                predicate = Utility.And(predicate, x => x.farms.SelectMany(c => c.cropacreages.Select(y => y.crop.name.ToLower())).Contains(searchViewModel.enterprise));
            }

            if (searchViewModel.gender != null && searchViewModel.gender != "all")
                predicate = Utility.And(predicate, x => x.gender == searchViewModel.gender);

            if (searchViewModel.scale != null && searchViewModel.scale != "all")
            {
                if (searchViewModel.scale.ToLower() == "large")
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

        [HttpPost]
        [Route("corporate")]
        public IHttpActionResult AddFarmerCorporate(AddFarmerCorporateViewModel newfarmer)
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
            db.SaveChanges();

            //ADIMS ID
            farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";
            
            db.Entry(farmer).State = EntityState.Modified;

            db.SaveChanges();
           
            return Ok("Corporate Farmer Added Successfully");
        }

        [HttpPost]
        [Route("group")]
        public IHttpActionResult AddFarmerGroup(AddFarmerGroupViewModel newfarmer)
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
            db.SaveChanges();

            //ADIMS ID
            farmer.adimsid = $"DA/{county?.code ?? "00"}/{ward?.code ?? "00"}/{farmer?.id}";
            

            db.Entry(farmer).State = EntityState.Modified;

            db.SaveChanges();
            
            return Ok("Group Farmer Added Successfully");
        }

    }
}
