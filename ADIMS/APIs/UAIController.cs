using ADIMS.Models;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using WebApi.OutputCache.V2;

namespace ADIMS.APIs
{
    [RoutePrefix("api/uais")]
    public class UAIController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [Route("get/{ward}")]
        [HttpGet]
        public IHttpActionResult GetUAIs(int ward)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var _ward = db.wards.Include(x => x.uais)
                                .FirstOrDefault(x => x.id == ward);
            
            var _uais = _ward.uais.ToList();

            return Ok(_uais);
        }

        [Route("getall")]
        [HttpGet]
        public IHttpActionResult GetUAIs()
        {
            db.Configuration.LazyLoadingEnabled = false;
            
            var _uais = db.uais.Include(x => x.ward1)
                               .ToList();

            return Ok(_uais);
        }

        [Route("uaiqueues/{uai}")]
        [HttpGet]
        public IHttpActionResult GetQueues(int uai)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var _uai = db.uais.Include(x => x.crop_cutting_queue)
                              .Include(x => x.ward1)
                              .FirstOrDefault(x => x.id == uai);

            var queues = _uai.crop_cutting_queue.Where(x => x.isdeleted != true).ToList();

            return Ok(queues);
        }

        [Route("farmsinqueue/{queue}")]
        [HttpGet]
        public IHttpActionResult GetQueueFarms(int queue)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var _queue = db.crop_cutting_queue
                           .Include(x => x.cce_queue_farms)
                           .FirstOrDefault(x => x.id ==  queue && x.isdeleted != true);

            var farms = _queue.cce_queue_farms.ToList();

            return Ok(farms);
        }
    }
}
