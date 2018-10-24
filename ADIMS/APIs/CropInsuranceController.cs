using ADIMS.Models;
using System;
using System.Web.Http;

namespace ADIMS.APIs
{
    [RoutePrefix("api/cropinsurance")]
    public class CropInsuranceController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();
        
        [HttpGet]
        [Route("addtoqueue")]
        public IHttpActionResult AddToQueue(string queue, string cropacreage)
        {
            cce_queue_farms _cqf = new cce_queue_farms()
            {
                queue = Convert.ToInt32(queue),
                cropacreage = Convert.ToInt32(cropacreage),
                datecreated = DateTime.Now,
                createdby = 1
            };

            db.cce_queue_farms.Add(_cqf);
            db.SaveChanges();
            
            return Ok("Successfully Added to Queue");
        }
    }
}
