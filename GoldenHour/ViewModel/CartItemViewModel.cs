namespace GoldenHour.ViewModel
{
    public class CartItemViewModel : ViewModelBase
    {
        private ProductViewModel _product;
        public ProductViewModel Product
        {
            get => _product;
            set { _product = value; OnPropertyChanged(nameof(Product)); }
        }

        private int _quantity;
        public decimal SubTotal => (Product?.Total ?? 0) * Quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(SubTotal));
            }
        }
    }
}
