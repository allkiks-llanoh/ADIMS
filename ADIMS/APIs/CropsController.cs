using ADIMS.Models;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ADIMS.APIs
{
    [RoutePrefix("api/crops")]
    [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
    public class CropsController : ApiController
    {
        public readonly adimsEntities db = new adimsEntities();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCrops()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.crops.ToList());
        }

        [HttpGet]
        [Route("{cropid}")]
        public IHttpActionResult GetCropVarieties(int cropid)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var varieties = db.cropvarieties.Where(x => x.cropid == cropid).ToList();

            return Ok(varieties);
        }
    }
}
