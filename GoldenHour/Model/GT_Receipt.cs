using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_Receipt
    {
        public int rec_idReceipt { get; set; }
        public decimal? rec_total { get; set; }
        public string rec_client { get; set; }
        public string rec_mail { get; set; }
        public DateTime? rec_date { get; set; }
        public int? rec_status { get; set; }
        public int? fk_idUser { get; set; }
        public int? fk_idPayment { get; set; }
        public int? fk_idModifier { get; set; }
    }
}
