using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_MeanPayment
    {
        public int mep_idMeanPayment { get; set; }
        public string mep_name { get; set; }
        public int? mep_status { get; set; }
        public DateTime? mep_date { get; set; }
    }
}
