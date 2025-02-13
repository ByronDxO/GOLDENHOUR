using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_User
    {
        public int usu_idUser { get; set; }
        public string usu_firstname { get; set; }
        public string usu_lastName { get; set; }
        public string usu_password { get; set; }
        public string usu_username { get; set; }
        public DateTime? usu_dateIni { get; set; }
        public int? usu_status { get; set; }
        public string mail { get; set; }
    }
}
