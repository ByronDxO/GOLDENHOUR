using GoldenHour.Repositories;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;


namespace GoldenHour.ViewModel
{
    public class GenerateReceiptViewModel : ViewModelBase
    {  // Lista de categorías (suponiendo que CategoryViewModel tenga al menos la propiedad Name)
        public ObservableCollection<CategoryViewModel> Categories { get; set; }

        // Categoría seleccionada
        private CategoryViewModel _selectedCategory;
        public CategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                // Al cambiar la categoría, se carga la lista de productos correspondiente
                LoadProductsForCategory();
            }
        }


        public ObservableCollection<ModifierViewModel> Modifiers { get; set; }

        private ModifierViewModel _selectedModifier;
        public ModifierViewModel SelectedModifier
        {
            get => _selectedModifier;
            set
            {
                _selectedModifier = value;
                OnPropertyChanged(nameof(SelectedModifier));
                OnPropertyChanged(nameof(FinalTotal));
            }
        }

        private bool _showModifierPanel = false;
        public bool ShowModifierPanel
        {
            get => _showModifierPanel;
            set { _showModifierPanel = value; OnPropertyChanged(nameof(ShowModifierPanel)); }
        }

        public decimal FinalTotal => SelectedModifier == null
            ? TotalPrice
            : TotalPrice - (TotalPrice * SelectedModifier.Percentage / 100);


      

        public decimal TotalPrice
        {
            get => CartItems.Sum(item => (item.Product?.Total ?? 0) * item.Quantity);
        }


        // Lista de productos que se muestran (correspondientes a la categoría seleccionada)
        public ObservableCollection<ProductViewModel> Products { get; set; }

        // Carrito: productos agregados
        public ObservableCollection<CartItemViewModel> CartItems { get; set; }

        // Comandos
        public ICommand AddToCartCommand { get; }
        public ICommand ContinueCommand { get; }
        public ICommand SelectCategoryCommand { get; } // Opcional, si quieres usarlo en lugar de manejar el click directo


        public ICommand RemoveFromCartCommand { get; }


        public ICommand GenerateReceiptCommand { get; }
        public GenerateReceiptViewModel()
        {
            // Inicializamos las colecciones
            Categories = new ObservableCollection<CategoryViewModel>();
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<CartItemViewModel>();

            Modifiers = new ObservableCollection<ModifierViewModel>();

            CartItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalPrice));


            // Inicializamos comandos
            SelectCategoryCommand = new ViewModelCommand(param =>
            {
                SelectedCategory = param as CategoryViewModel;
            });
            AddToCartCommand = new ViewModelCommand(param => AddProductToCart(param as ProductViewModel));
            ContinueCommand = new ViewModelCommand(param => ShowModifierPanel = true);
            RemoveFromCartCommand = new ViewModelCommand(param => RemoveProductFromCart(param as CartItemViewModel));
            GenerateReceiptCommand = new ViewModelCommand(param => GenerateAndPrintReceipt());


            // Cargar las categorías desde el repositorio o servicio
            LoadCategories();
            LoadModifiers();
        }

        private void RemoveProductFromCart(CartItemViewModel item)
        {
            if (item != null)
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        private void LoadCategories()
        {
            var catRepo = new CategoryRepository();
            // Obtén la lista de GT_Category desde la base de datos
            var catList = catRepo.GetCategories();
            // Mapea cada GT_Category a CategoryViewModel
            foreach (var cat in catList)
            {
                Categories.Add(new CategoryViewModel
                {
                    Id = cat.cat_idCategory,
                    Name = cat.cat_name,
                    Description = cat.cat_description
                });
            }
        }

        private void LoadProductsForCategory()
        {
            Products.Clear();
            if (SelectedCategory != null)
            {
                // Aquí llamamos al repositorio para obtener los productos de la categoría seleccionada.
                // Por ejemplo, suponiendo que tienes un método GetProductsByCategory en ProductRepository:
                var prodRepo = new ProductRepository();
                var prodList = prodRepo.GetProductsByCategory(SelectedCategory.Id);
                foreach (var p in prodList)
                {
                    // Mapea la entidad GT_Product a tu ProductViewModel.
                    Products.Add(new ProductViewModel
                    {
                        Id = p.pro_idProduct,
                        Name = p.pro_name,
                        ImagePath = p.pro_pathImage,
                        Total = p.pro_total,
                        Stock = p.pro_stock
                    });
                }
            }
        }

        private void AddProductToCart(ProductViewModel product)
        {
            if (product != null)
            {
                // Opcional: Si el producto ya está en el carrito, aumenta la cantidad.
                var existingItem = CartItems.FirstOrDefault(item => item.Product.Id == product.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;

                    OnPropertyChanged(nameof(TotalPrice));
                }
                else
                {
                    // Si no existe, agrega un nuevo elemento al carrito con cantidad 1.
                    CartItems.Add(new CartItemViewModel { Product = product, Quantity = 1 });
                }
            }
        }

        private void LoadModifiers()
        {
            var modRepo = new ModifierRepository();
            var mods = modRepo.GetModifiers();
            foreach (var mod in mods)
            {
                Modifiers.Add(new ModifierViewModel
                {
                    Id = mod.mdf_idModifier,
                    Name = mod.mdf_name,
                    Percentage = mod.mdf_percentage.HasValue ? mod.mdf_percentage.Value : 0
                });
            }
        }


        private void ContinueToReceipt()
        {
            
        }



        public void GenerateAndPrintReceipt()
        {
            try
            {
                // Crear un documento PDF nuevo
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Recibo (Documento no Tributario)";

                // Crear una página con ancho máximo de 55mm (~156 puntos)
                PdfPage page = document.AddPage();
                double widthInPoints = 55 * 2.83465; // Aproximadamente 156 puntos
                page.Width = widthInPoints;
                // Establecer una altura adecuada (puedes calcularla o fijarla)
                page.Height = 400;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Definir las fuentes (ajusta según tus preferencias)
                XFont fontTitle = new XFont("Arial", 10, PdfSharp.Drawing.XFontStyleEx.Bold);
                XFont fontRegular = new XFont("Arial", 8, PdfSharp.Drawing.XFontStyleEx.Regular);






                // Posiciones iniciales (márgenes)
                double posX = 5, posY = 5;

                // El URI pack indica que la imagen se encuentra en el assembly actual, en la carpeta "images".
                Uri uri = new Uri("pack://application:,,,/images/logo4.jpg", UriKind.Absolute);
                var resourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (resourceInfo == null)
                    throw new InvalidOperationException("No se encontró el recurso de la imagen.");
                XImage logo = XImage.FromStream(resourceInfo.Stream);



                // Definir posición y tamaño para el logo (ajusta según tus necesidades)
                // Por ejemplo, dibujar el logo en la parte superior centrado, con un ancho de 100 puntos y altura proporcional.
                double logoWidth = 100;
                double logoHeight = logo.PixelHeight * logoWidth / logo.PixelWidth; // Mantiene la proporción
                double logoPosX = (page.Width - logoWidth) / 2; // Centrado horizontalmente
                double logoPosY = 10; // Un margen superior de 10 puntos

                // Dibujar el logo en la página
                gfx.DrawImage(logo, logoPosX, logoPosY, logoWidth, logoHeight);

                // Actualizar posY para continuar con el contenido debajo del logo
                posY = logoPosY + logoHeight + 10;

                // Dibujar encabezado
                gfx.DrawString("Recibo", fontTitle, XBrushes.Black, new XRect(0, posY, page.Width, 20), XStringFormats.TopCenter);
                posY += 25;

                // Dibujar fecha
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                gfx.DrawString($"Fecha: {fecha}", fontRegular, XBrushes.Black, posX, posY);
                posY += 15;

                // Dibujar los ítems del carrito
                foreach (var item in CartItems)
                {
                    // Truncar el nombre si es muy largo
                    string nombre = item.Product.Name.Length > 15 ? item.Product.Name.Substring(0, 15) + "..." : item.Product.Name;
                    gfx.DrawString(nombre, fontRegular, XBrushes.Black, posX, posY);
                    posY += 10;
                    gfx.DrawString($"Cant: {item.Quantity}", fontRegular, XBrushes.Black, posX, posY);
                    posY += 10;
                    gfx.DrawString($"Subtotal: Q{item.SubTotal:N2}", fontRegular, XBrushes.Black, posX, posY);
                    posY += 15;
                }

                // Dibujar totales
                gfx.DrawString($"Total: Q{TotalPrice:N2}", fontTitle, XBrushes.Black, posX, posY);
                posY += 15;
                gfx.DrawString($"Total Final: Q{FinalTotal:N2}", fontTitle, XBrushes.Black, posX, posY);
                posY += 20;
                gfx.DrawString("Gracias por su compra", fontRegular, XBrushes.Black, posX, posY);

                // Guardar el documento en un archivo temporal
                string tempPath = Path.Combine(Path.GetTempPath(), "Recibo.pdf");
                document.Save(tempPath);

                // Imprimir el PDF usando la aplicación predeterminada para PDF (o un proceso específico)
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true // Esto abrirá el PDF con la aplicación predeterminada, que puede tener opción de imprimir
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes mostrar un mensaje o registrar el error)
                Debug.WriteLine("Error al generar e imprimir el recibo: " + ex.Message);
            }
        }

    }
}