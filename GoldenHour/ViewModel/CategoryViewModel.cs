namespace GoldenHour.ViewModel
{
    public class CategoryViewModel : ViewModelBase
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

        // Puedes agregar más propiedades si lo requieres, por ejemplo, descripción.
        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
    }
}
