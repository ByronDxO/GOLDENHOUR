using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class DailySalesReport
    {
        public DateTime ReportDate { get; set; }
        public int TotalReceipts { get; set; }
        public decimal TotalSales { get; set; }
    }
}
