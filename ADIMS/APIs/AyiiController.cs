using ADIMS.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace ADIMS.APIs
{
    [RoutePrefix("api/ayii")]
    public class AyiiController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();
        
        [HttpGet]
        [Route("averageyield")]
        public IHttpActionResult GetAverageYield(string uai, int season, int year)
        {
            //Return the average yield for the parameters above, if none, return 0
            double acre_area = 4048.5;
            double sample_area = 25;
            double kg_per_bag = 90;
            

            //search for the records here
            try
            {
                var queue = db.crop_cutting_queue
                                  .FirstOrDefault(x => x.uai1.code == uai && x.season == season && x.year == year);

                var sample_dry_weight = queue.cce_queue_farms
                                        .Average(x => x.actual_cropcutting_exercise?.quantity_harvested_wetgrain * (100 - x.actual_cropcutting_exercise?.moisture_reading) /100) ?? 0;

                var acre_yield = (acre_area * Convert.ToDouble(sample_dry_weight)) / sample_area;

                if (acre_yield == double.NaN)
                    acre_yield = 0;

                double bags = acre_yield / kg_per_bag;
                
                return Ok( Math.Round(bags,0));
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }
    }
}
