using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.ViewModel
{
    public class MeanPaymentViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private int? _status;
        public int? Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
    }
}
