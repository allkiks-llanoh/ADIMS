using ADIMS.Models;

namespace ADIMS.Services
{
    public class DataHelper
    {
        public static string GetSubCountyName(int? subcounty)
        {
            if (subcounty == null)
                return "";
            string sub_county = "";
            using (var db = new adimsEntities())
            {
                sub_county = db.subcounties.Find(subcounty)?.name;
            }
            return sub_county;
        }
    }
}