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
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Printing;
using System.Windows.Xps;


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

        public ObservableCollection<MeanPaymentViewModel> PaymentMethods { get; set; }

        private MeanPaymentViewModel _selectedPaymentMethod;
        public MeanPaymentViewModel SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set
            {
                _selectedPaymentMethod = value;
                OnPropertyChanged(nameof(SelectedPaymentMethod));
                OnPropertyChanged(nameof(CanGenerateReceipt));
            }
        }


        private string _transactionNumber;
        public string TransactionNumber
        {
            get => _transactionNumber;
            set
            {
                _transactionNumber = value;
                OnPropertyChanged(nameof(TransactionNumber));
                OnPropertyChanged(nameof(CanGenerateReceipt));
            }
        }


        public bool CanGenerateReceipt
        {
            get
            {
                // Se requiere que haya un modificador y un medio de pago seleccionado.
                if (SelectedModifier == null || SelectedPaymentMethod == null)
                    return false;

                // Si el medio de pago es "Efectivo" (ID == 1), no se requiere el número de transacción.
                if (SelectedPaymentMethod.Id == 1)
                    return true;
                else
                    // Si no es efectivo, se requiere que se ingrese el número de transacción.
                    return !string.IsNullOrWhiteSpace(TransactionNumber);
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
                OnPropertyChanged(nameof(CanGenerateReceipt));
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
        public ICommand PrintReceiptCommand { get; }

        public ICommand RemoveFromCartCommand { get; }

        public ICommand NewReceiptCommand { get; }

        public ICommand GenerateReceiptCommand { get; }
        public GenerateReceiptViewModel()
        {
            // Inicializamos las colecciones
            Categories = new ObservableCollection<CategoryViewModel>();
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<CartItemViewModel>();

            Modifiers = new ObservableCollection<ModifierViewModel>();
            PaymentMethods = new ObservableCollection<MeanPaymentViewModel>();  // Nueva colección

            CartItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalPrice));


            // Inicializamos comandos
            SelectCategoryCommand = new ViewModelCommand(param =>
            {
                SelectedCategory = param as CategoryViewModel;
            });
            AddToCartCommand = new ViewModelCommand(param => AddProductToCart(param as ProductViewModel));
            ContinueCommand = new ViewModelCommand(param => ShowModifierPanel = true);
            RemoveFromCartCommand = new ViewModelCommand(param => RemoveProductFromCart(param as CartItemViewModel));
            

            PrintReceiptCommand = new ViewModelCommand(
                param => RegisterPaymentAndPrintReceipt(),
                param => CanGenerateReceipt
            );

            NewReceiptCommand = new ViewModelCommand(param => ResetReceipt());


            // Cargar las categorías desde el repositorio o servicio
            LoadCategories();
            LoadModifiers();
            LoadPaymentMethods();
        }


        private void ResetReceipt()
        {
            // Vaciar el carrito
            CartItems.Clear();

            // Reiniciar selecciones de modificador, medio de pago y número de transacción
            SelectedModifier = null;
            SelectedPaymentMethod = null;
            TransactionNumber = string.Empty;

            // Ocultar el panel de modificadores
            ShowModifierPanel = false;

            // Si tienes otros campos (por ejemplo, datos del cliente) resétalos también aquí.
            // También podrías limpiar Products, si se requiere volver a seleccionar una categoría, etc.
        }

        private void RemoveProductFromCart(CartItemViewModel item)
        {
            if (item != null)
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(TotalPrice));
            }
        }


        private void LoadPaymentMethods()
        {
            PaymentMethods.Clear();
            var paymentRepo = new MeanPaymentRepository();
            var payments = paymentRepo.GetMeanPayments(); // Obtiene una colección de GT_MeanPayment
            foreach (var p in payments)
            {
                PaymentMethods.Add(new MeanPaymentViewModel
                {
                    Id = p.mep_idMeanPayment,
                    Name = p.mep_name,
                    Status = p.mep_status,
                    Date = p.mep_date
                });
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


        private void RegisterPaymentAndPrintReceipt()
        {
            try
            {
                // Registrar el medio de pago
                RegisterPayment();
                SaveReceiptAndDetails();
                // Luego, generar e imprimir el recibo (usando el método que ya tienes)
                PrintReceiptUsingFlowDocument();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al registrar el medio de pago y/o imprimir el recibo: " + ex.Message);
            }
        }

        private void RegisterPayment()
        {
            var paymentRepo = new Repositories.PaymentRepository();
            // Si el medio de pago es "Efectivo", se asigna "0"; si no, se usa TransactionNumber
            string transactionNumber = "0";
            if (SelectedPaymentMethod != null &&
                !string.Equals(SelectedPaymentMethod.Name?.Trim(), "Efectivo", StringComparison.OrdinalIgnoreCase))
            {
                transactionNumber = TransactionNumber; // Aquí asumes que el usuario ingresó el número de transacción.
            }

            var payment = new GoldenHour.Model.GT_Payment
            {
                pay_transaction = transactionNumber,
                pay_date = DateTime.Now,
                fk_idMeanPayment = SelectedPaymentMethod?.Id
            };

            int insertedId = paymentRepo.InsertPayment(payment);
            // Puedes validar que insertedId sea mayor que 0 para confirmar la inserción.
        }




        public void PrintReceiptUsingFlowDocument()
        {
            try {
            // Crear un FlowDocument y configurar sus propiedades
            FlowDocument doc = new FlowDocument();
            doc.PageWidth = 156; // 55 mm ≈ 156 puntos
            doc.PagePadding = new Thickness(10);
            doc.ColumnGap = 0;
            doc.ColumnWidth = doc.PageWidth;

            // 1. Agregar el logo desde los recursos
            // Usamos un Pack URI para acceder al recurso embebido (asegúrate de que el archivo logo4.jpg esté configurado como Resource)
            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/images/logo5.jpg", UriKind.Absolute));
            System.Windows.Controls.Image logoControl = new System.Windows.Controls.Image();
            logoControl.Source = bitmap;
            logoControl.Width = 100; // Ajusta el ancho deseado
            logoControl.Stretch = Stretch.Uniform;
            logoControl.HorizontalAlignment = HorizontalAlignment.Center; // Establecer aquí la alineación
            BlockUIContainer logoContainer = new BlockUIContainer(logoControl);
            doc.Blocks.Add(logoContainer);


            // Agregar un espacio (salto de línea)
            doc.Blocks.Add(new Paragraph(new Run("\n")));

            // 2. Encabezado
            Paragraph header = new Paragraph(new Run("Recibo (Documento no Tributario)"));
            header.FontSize = 14;
            header.FontWeight = FontWeights.Bold;
            header.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(header);

            // 3. Fecha
            Paragraph fecha = new Paragraph(new Run("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
            fecha.FontSize = 10;
            doc.Blocks.Add(fecha);

            // 4. Ítems del carrito
            foreach (var item in CartItems)
            {
                string nombre = item.Product.Name.Length > 15
                    ? item.Product.Name.Substring(0, 15) + "..."
                    : item.Product.Name;
                Paragraph pItem = new Paragraph(new Run($"{nombre} - Cant: {item.Quantity} - Subtotal: Q{item.SubTotal:N2}"));
                pItem.FontSize = 10;
                doc.Blocks.Add(pItem);
            }

            // 5. Totales
            Paragraph totalP = new Paragraph(new Run($"Total: Q{TotalPrice:N2}"));
            totalP.FontSize = 12;
            totalP.FontWeight = FontWeights.Bold;
            doc.Blocks.Add(totalP);

            Paragraph finalTotalP = new Paragraph(new Run($"Total Final: Q{FinalTotal:N2}"));
            finalTotalP.FontSize = 12;
            finalTotalP.FontWeight = FontWeights.Bold;
            doc.Blocks.Add(finalTotalP);

            // 6. Mensaje final
            Paragraph agradecimiento = new Paragraph(new Run("Gracias por su compra"));
            agradecimiento.FontSize = 10;
            doc.Blocks.Add(agradecimiento);

            // 7. Imprimir el FlowDocument sin mostrar diálogo
            PrintFlowDocument(doc);
            }
            catch (Exception e) {
                Debug.WriteLine("Error al generar e imprimir el recibo: " + e.Message);
            }
        }

        private void PrintFlowDocument(FlowDocument doc)
        {
            // Obtener la impresora predeterminada
            LocalPrintServer localPrintServer = new LocalPrintServer();
            PrintQueue printQueue = localPrintServer.DefaultPrintQueue;

            // Crear un XpsDocumentWriter para enviar el documento a la impresora
            XpsDocumentWriter xpsWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);

            // Imprimir el documento sin mostrar diálogo
            xpsWriter.Write(((IDocumentPaginatorSource)doc).DocumentPaginator);
        }


        private void SaveReceiptAndDetails()
        {
            // 1. Crear el objeto GT_Receipt con la información del recibo.
            // Por ejemplo, supongamos que en tu app tienes información del cliente y del usuario actual:
            var receipt = new GoldenHour.Model.GT_Receipt
            {
                rec_total = TotalPrice,  // O FinalTotal, según cómo se quiera guardar
                rec_client = "Nombre del Cliente",  // Reemplaza con la información real
                rec_mail = "cliente@correo.com",      // Reemplaza con la información real
                rec_date = DateTime.Now,
                rec_status = 1, // Por ejemplo, 1 = activo
                fk_idUser = 1,  // Reemplaza con el ID del usuario actual
                fk_idPayment = SelectedPaymentMethod?.Id, // Registrado en el pago
                fk_idModifier = SelectedModifier?.Id
            };

            // 2. Insertar el recibo y capturar el ID insertado
            var receiptRepo = new ReceiptRepository();
            int receiptId = receiptRepo.InsertReceipt(receipt);

            // 3. Por cada producto en el carrito, insertar un detalle
            var detailRepo = new DetailReceiptRepository();
            foreach (var item in CartItems)
            {
                var detail = new GoldenHour.Model.GT_DetailReceipt
                {
                    dre_date = DateTime.Now,
                    dre_stock = item.Quantity, // O la cantidad que corresponda (puede representar el stock vendido)
                    dre_total = item.SubTotal,
                    fk_idProduct = item.Product.Id,
                    fk_idReceipt = receiptId
                };
                detailRepo.InsertDetailReceipt(detail);
            }
        }



    }
}