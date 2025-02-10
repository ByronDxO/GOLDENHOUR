using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_Category
    {
        public int cat_idCategory { get; set; }
        public string cat_name { get; set; }
        public string cat_description { get; set; }
        public int? cat_status { get; set; }
        public DateTime? cat_date { get; set; }

        public override string ToString()
        {
            return cat_name;
        }
    }
}
