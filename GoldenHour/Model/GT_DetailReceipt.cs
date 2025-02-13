using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_DetailReceipt
    {
        public int dre_idDetailReceipt { get; set; }
        public DateTime? dre_date { get; set; }
        public int? dre_stock { get; set; }
        public decimal? dre_total { get; set; }
        public int? fk_idProduct { get; set; }
        public int? fk_idReceipt { get; set; }
    }
}

