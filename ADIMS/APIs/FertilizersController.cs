using ADIMS.Models;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ADIMS.APIs
{
    [RoutePrefix("api/fertilizers")]
    [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
    public class FertilizersController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [Route("planting")]
        public IHttpActionResult GetPlantingFertilizers()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.fertilizers.Where(x => x.application_method == "Planting").ToList());
        }

        [Route("topdressing")]
        public IHttpActionResult GetTopDressingFertilizers()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.fertilizers.Where(x => x.application_method == "TopDressing").ToList());
        }

    }
}
