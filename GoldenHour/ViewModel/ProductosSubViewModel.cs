using GoldenHour.Model;
using GoldenHour.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Microsoft.Win32;
using System.IO;
namespace GoldenHour.ViewModel
{
    public class ProductosSubViewModel : ViewModelBase
    {
        // Propiedad para mostrar mensajes en la vista
        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); }
        }

        // Propiedad para el nombre del producto
        private string _productName;
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }

        // Propiedad para la descripción del producto
        private string _productDescription;
        public string ProductDescription
        {
            get => _productDescription;
            set { _productDescription = value; OnPropertyChanged(nameof(ProductDescription)); }
        }

        // Propiedad para el total del producto
        private decimal? _productTotal;
        public decimal? ProductTotal
        {
            get => _productTotal;
            set { _productTotal = value; OnPropertyChanged(nameof(ProductTotal)); }
        }

        // Propiedad para el stock del producto
        private int? _productStock;
        public int? ProductStock
        {
            get => _productStock;
            set { _productStock = value; OnPropertyChanged(nameof(ProductStock)); }
        }

        // Propiedad para la ruta de la imagen
        private string _productPathImage;
        public string ProductPathImage
        {
            get => _productPathImage;
            set { _productPathImage = value; OnPropertyChanged(nameof(ProductPathImage)); }
        }

        // Nueva: Colección de categorías disponibles
        private IEnumerable<GT_Category> _categories;
        public IEnumerable<GT_Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        // Nueva: Categoría seleccionada en el ComboBox
        private GT_Category _selectedCategory;
        public GT_Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }

        // Comando para guardar el producto
        public ICommand SaveProductCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ProductosSubViewModel()
        {
            Mensaje = "Ingrese los datos del producto";
            SaveProductCommand = new ViewModelCommand(_ => SaveProduct(), _ => !string.IsNullOrWhiteSpace(ProductName));
            UploadImageCommand = new ViewModelCommand(_ => UploadImage());
            // Cargar las categorías existentes desde la base de datos
            var catRepo = new CategoryRepository();
            Categories = catRepo.GetCategories().ToList();
        }

        private void SaveProduct()
        {
            try
            {
                var repository = new ProductRepository();
                var newProduct = new GT_Product
                {
                    pro_name = ProductName,
                    pro_description = ProductDescription,
                    pro_status = 1, // Por ejemplo, 1 = activo
                    pro_date = DateTime.Now,
                    pro_total = ProductTotal,
                    pro_stock = ProductStock,
                    pro_pathImage = ProductPathImage,
                    // Se asigna el ID de categoría según la categoría seleccionada en el ComboBox
                    fk_category = SelectedCategory?.cat_idCategory
                };

                int result = repository.InsertProduct(newProduct);
                if (result > 0)
                {
                    Mensaje = "Producto guardado correctamente.";
                    // Limpia los campos para ingresar un nuevo producto.
                    ProductName = string.Empty;
                    ProductDescription = string.Empty;
                    ProductTotal = null;
                    ProductStock = null;
                    ProductPathImage = string.Empty;
                    SelectedCategory = null;
                }
                else
                {
                    Mensaje = "No se pudo guardar el producto.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al guardar producto: " + ex.Message;
            }
        }



        private void UploadImage()
        {
            try
            {
                // Crear y configurar el OpenFileDialog.
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                if (dlg.ShowDialog() == true)
                {
                    string sourceFile = dlg.FileName;
                    // Define la carpeta destino (por ejemplo, "Images" en la carpeta de la aplicación)
                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    // Puedes usar el mismo nombre o generar uno único.
                    string fileName = Path.GetFileName(sourceFile);
                    string destPath = Path.Combine(folder, fileName);
                    // Copia el archivo (si ya existe, sobrescribe)
                    File.Copy(sourceFile, destPath, true);
                    // Guarda la ruta en la propiedad (puedes guardar la ruta completa o relativa)
                    ProductPathImage = destPath;
                    Mensaje = "Imagen subida correctamente.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error subiendo la imagen: " + ex.Message;
            }
        }

    }
}
