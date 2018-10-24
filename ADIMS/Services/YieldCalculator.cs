using ADIMS.Models;
using System;
using System.Linq;

namespace ADIMS.Services
{
    public class YieldCalculator
    {
        private readonly adimsEntities db = new adimsEntities();
        public static double GetAverageYield(string uai, int season, int year)
        {
            
        //Return the average yield for the parameters above, if none, return 0
            double acre_area = 4046.656;
            double sample_area = 25;
            double kg_per_bag = 90;

            decimal moisture_percentage = 15;

            double acre_yield;

            double bags;

            //search for the records here
            try
            {
                using (var db = new adimsEntities())
                {
                    var queue = db.crop_cutting_queue
                                 .FirstOrDefault(x => x.uai1.code == uai && x.season == season && x.year == year);

                    var sample_dry_weight = queue.cce_queue_farms
                                            .Average(x => x.actual_cropcutting_exercise?.quantity_harvested_wetgrain * (moisture_percentage / 100) / (x.actual_cropcutting_exercise?.moisture_reading / 100)) ?? 0;

                     acre_yield = (acre_area * Convert.ToDouble(sample_dry_weight)) / sample_area;
                    if (acre_yield == double.NaN)
                        acre_yield = 0;

                    bags = acre_yield / kg_per_bag;
                    
                }
                return Math.Round(bags, 0);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}