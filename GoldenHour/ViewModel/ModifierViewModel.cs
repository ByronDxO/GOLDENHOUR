using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.ViewModel
{
    public class ModifierViewModel : ViewModelBase
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

        private decimal _percentage;
        public decimal Percentage
        {
            get => _percentage;
            set { _percentage = value; OnPropertyChanged(nameof(Percentage)); }
        }

        // Este método override es útil para que, al mostrar el objeto en controles
        // como ComboBox, se muestre una representación legible.
        public override string ToString()
        {
            return $"{Name} ({Percentage}%)";
        }
    }
}