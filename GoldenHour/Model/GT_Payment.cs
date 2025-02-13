using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_Payment
    {
        public int pay_idPayment { get; set; }
        public string pay_transaction { get; set; }
        public DateTime? pay_date { get; set; }
        public int? fk_idMeanPayment { get; set; }
    }
}
