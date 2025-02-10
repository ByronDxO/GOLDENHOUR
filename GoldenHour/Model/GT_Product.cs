using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_Product
    {

        public int pro_idProduct { get; set; }
        public string pro_name { get; set; }
        public string pro_description { get; set; }
        public int? pro_status { get; set; }
        public DateTime? pro_date { get; set; }
        public decimal? pro_total { get; set; }
        public int? pro_stock { get; set; }
        public string pro_pathImage { get; set; }
        public int? fk_category { get; set; }

    }
}
