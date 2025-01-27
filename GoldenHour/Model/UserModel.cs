using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public  class UserModel
    {

        public int usu_IdUser { get; set; }

        public string usu_Firstname { get; set; }

        public string usu_LastName { get; set; }

        public string usu_Password { get; set; }

        public string usu_Username { get; set; }

        public DateTime usu_DateIni { get; set; }

        public int usu_Status { get; set; }


        public string mail { get; set; }
    }
}
