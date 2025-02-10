using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Model
{
    public class GT_Modifier
    {
        public int mdf_idModifier { get; set; }
        public string mdf_name { get; set; }
        public string mdf_description { get; set; }
        public int? mdf_status { get; set; }
        public int? mdf_percentage { get; set; }

        // Para que al mostrar el objeto se vea el nombre
        public override string ToString()
        {
            return mdf_name;
        }
    }
}
