using ADIMS.Models;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ADIMS.APIs
{
    [RoutePrefix("api/counties")]
    [CacheOutput(ClientTimeSpan = 1000000, ServerTimeSpan = 1000000)]
    public class CountiesController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var counties = db.counties.OrderBy(x => x.name).ToList();

            return Ok(counties);
        }

        [HttpGet]
        [Route("{id}/subcounties")]
        public IHttpActionResult GetSubCounties(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var sub_counties = db.subcounties.OrderBy(x => x.name).Where(x => x.countyid == id).ToList();

            return Ok(sub_counties);
        }

        [HttpGet]
        [Route("{id}/wards")]
        public IHttpActionResult GetWards(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;

            var wards = db.wards.OrderBy(x => x.name).Where(x => x.subcounty == id).ToList();

            return Ok(wards);
        }
    }
}
