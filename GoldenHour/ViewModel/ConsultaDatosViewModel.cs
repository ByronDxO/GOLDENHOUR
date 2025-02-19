using GoldenHour.Model;
using GoldenHour.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GoldenHour.ViewModel
{
    public class ConsultaDatosViewModel : ViewModelBase
    {
        // Colecciones de datos
        public ObservableCollection<GT_Receipt> Receipts { get; set; }
        public ObservableCollection<UserAccountModel> Employees { get; set; }
        public ObservableCollection<GT_Product> Products { get; set; }
        public ObservableCollection<GT_Category> Categories { get; set; }
        public ObservableCollection<GT_Modifier> Modifiers { get; set; }
        public ObservableCollection<GT_MeanPayment> MeanPayments { get; set; }

        // Reporte de Ventas Diarias
        public ObservableCollection<DailySalesReport> DailySalesReports { get; set; }
        private DateTime _salesReportDate = DateTime.Today;
        public DateTime SalesReportDate
        {
            get => _salesReportDate;
            set
            {
                _salesReportDate = value;
                OnPropertyChanged("SalesReportDate");
            }
        }
        public ICommand LoadDailySalesReportsCommand { get; }

        // Comandos para Productos
        public ICommand DeleteProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand InsertProductCommand { get; }

        // Comandos para Recibos
        public ICommand DeleteReceiptCommand { get; }
        public ICommand UpdateReceiptCommand { get; }

        // Comandos para Categorías
        public ICommand DeleteCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }

        // Comandos para Modificadores
        public ICommand DeleteModifierCommand { get; }
        public ICommand UpdateModifierCommand { get; }

        // Comandos para Medios de Pago
        public ICommand DeleteMeanPaymentCommand { get; }
        public ICommand UpdateMeanPaymentCommand { get; }

        public ConsultaDatosViewModel()
        {
            Receipts = new ObservableCollection<GT_Receipt>();
            Employees = new ObservableCollection<UserAccountModel>();
            Products = new ObservableCollection<GT_Product>();
            Categories = new ObservableCollection<GT_Category>();
            Modifiers = new ObservableCollection<GT_Modifier>();
            MeanPayments = new ObservableCollection<GT_MeanPayment>();
            DailySalesReports = new ObservableCollection<DailySalesReport>();

            // Cargar datos
            LoadReceipts();
            LoadEmployees();
            LoadProducts();
            LoadCategories();
            LoadModifiers();
            LoadMeanPayments();

            // Inicializar comandos para productos
            DeleteProductCommand = new ViewModelCommand(param => DeleteProduct(param as GT_Product));
            UpdateProductCommand = new ViewModelCommand(param => UpdateProduct(param as GT_Product));
            InsertProductCommand = new ViewModelCommand(param => InsertProduct());

            // Inicializar comandos para recibos
            DeleteReceiptCommand = new ViewModelCommand(param => DeleteReceipt(param as GT_Receipt));
            UpdateReceiptCommand = new ViewModelCommand(param => UpdateReceipt(param as GT_Receipt));

            // Inicializar comandos para categorías
            DeleteCategoryCommand = new ViewModelCommand(param => DeleteCategory(param as GT_Category));
            UpdateCategoryCommand = new ViewModelCommand(param => UpdateCategory(param as GT_Category));

            // Inicializar comandos para modificadores
            DeleteModifierCommand = new ViewModelCommand(param => DeleteModifier(param as GT_Modifier));
            UpdateModifierCommand = new ViewModelCommand(param => UpdateModifier(param as GT_Modifier));

            // Inicializar comandos para medios de pago
            DeleteMeanPaymentCommand = new ViewModelCommand(param => DeleteMeanPayment(param as GT_MeanPayment));
            UpdateMeanPaymentCommand = new ViewModelCommand(param => UpdateMeanPayment(param as GT_MeanPayment));

            // Comando para reporte de ventas diarias
            LoadDailySalesReportsCommand = new ViewModelCommand(_ => LoadDailySalesReports());
        }

        #region Métodos de carga de datos

        private void LoadReceipts()
        {
            try
            {
                var receiptRepo = new ReceiptRepository();
                var list = receiptRepo.GetReceipts();
                Receipts.Clear();
                foreach (var rec in list)
                    Receipts.Add(rec);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando recibos: " + ex.Message);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                var userRepo = new UserRepository();
                var list = userRepo.GetAllEmployees();
                Employees.Clear();
                foreach (var emp in list)
                {
                    Employees.Add(new UserAccountModel
                    {
                        IdUser = emp.usu_idUser,
                        Username = emp.usu_username,
                        DisplayName = $"{emp.usu_firstname} {emp.usu_lastName}"
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando empleados: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var prodRepo = new ProductRepository();
                var list = prodRepo.GetAllProducts();
                Products.Clear();
                foreach (var prod in list)
                    Products.Add(prod);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando productos: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var catRepo = new CategoryRepository();
                var list = catRepo.GetCategories();
                Categories.Clear();
                foreach (var cat in list)
                    Categories.Add(cat);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando categorías: " + ex.Message);
            }
        }

        private void LoadModifiers()
        {
            try
            {
                var modRepo = new ModifierRepository();
                var list = modRepo.GetModifiers();
                Modifiers.Clear();
                foreach (var mod in list)
                    Modifiers.Add(mod);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando modificadores: " + ex.Message);
            }
        }

        private void LoadMeanPayments()
        {
            try
            {
                var meanPaymentRepo = new MeanPaymentRepository();
                var list = meanPaymentRepo.GetMeanPayments();
                MeanPayments.Clear();
                foreach (var mp in list)
                    MeanPayments.Add(mp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando medios de pago: " + ex.Message);
            }
        }

        private void LoadDailySalesReports()
        {
            try
            {
                var receiptRepo = new ReceiptRepository();
                var reports = receiptRepo.GetDailySalesReport(SalesReportDate);
                DailySalesReports.Clear();
                foreach (var rep in reports)
                    DailySalesReports.Add(rep);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando reporte de ventas diarias: " + ex.Message);
            }
        }

        #endregion

        #region Comandos de Productos

        private void DeleteProduct(GT_Product product)
        {
            if (product == null)
                return;
            try
            {
                var prodRepo = new ProductRepository();
                prodRepo.DeleteProduct(product.pro_idProduct);
                Products.Remove(product);
                MessageBox.Show("Producto eliminado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al eliminar producto: " + ex.Message);
            }
        }

        private void UpdateProduct(GT_Product product)
        {
            if (product == null)
                return;
            try
            {
                var prodRepo = new ProductRepository();
                prodRepo.UpdateProduct(product);
                MessageBox.Show("Producto actualizado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al actualizar producto: " + ex.Message);
            }
        }

        private void InsertProduct()
        {
            try
            {
                var prodRepo = new ProductRepository();
                GT_Product newProduct = new GT_Product
                {
                    pro_name = "Nuevo Producto",
                    pro_description = "Descripción del nuevo producto",
                    pro_status = 1,
                    pro_date = DateTime.Now,
                    pro_total = 100.00m,
                    pro_stock = 10,
                    pro_pathImage = "", // Ruta de imagen (opcional)
                    fk_category = Categories.FirstOrDefault()?.cat_idCategory
                };

                int insertedId = prodRepo.InsertProduct(newProduct);
                if (insertedId > 0)
                {
                    newProduct.pro_idProduct = insertedId;
                    Products.Add(newProduct);
                    MessageBox.Show("Producto insertado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo insertar el producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al insertar producto: " + ex.Message);
            }
        }

        #endregion

        #region Comandos de Recibos

        private void DeleteReceipt(GT_Receipt receipt)
        {
            if (receipt == null)
                return;
            try
            {
                var receiptRepo = new ReceiptRepository();
                receiptRepo.DeleteReceipt(receipt.rec_idReceipt);
                Receipts.Remove(receipt);
                MessageBox.Show("Recibo eliminado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el recibo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al eliminar recibo: " + ex.Message);
            }
        }

        private void UpdateReceipt(GT_Receipt receipt)
        {
            if (receipt == null)
                return;
            try
            {
                var receiptRepo = new ReceiptRepository();
                receiptRepo.UpdateReceipt(receipt);
                MessageBox.Show("Recibo actualizado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar el recibo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al actualizar recibo: " + ex.Message);
            }
        }

        #endregion

        #region Comandos de Categorías

        private void UpdateCategory(GT_Category category)
        {
            if (category == null)
                return;
            try
            {
                var catRepo = new CategoryRepository();
                catRepo.UpdateCategory(category);
                MessageBox.Show("Categoría actualizada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar la categoría: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al actualizar categoría: " + ex.Message);
            }
        }

        private void DeleteCategory(GT_Category category)
        {
            if (category == null)
                return;
            try
            {
                var catRepo = new CategoryRepository();
                catRepo.DeleteCategory(category.cat_idCategory);
                Categories.Remove(category);
                MessageBox.Show("Categoría eliminada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar la categoría: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al eliminar categoría: " + ex.Message);
            }
        }

        #endregion

        #region Comandos de Modificadores

        private void UpdateModifier(GT_Modifier modifier)
        {
            if (modifier == null)
                return;
            try
            {
                var modRepo = new ModifierRepository();
                modRepo.UpdateModifier(modifier);
                MessageBox.Show("Modificador actualizado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar el modificador: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al actualizar modificador: " + ex.Message);
            }
        }

        private void DeleteModifier(GT_Modifier modifier)
        {
            if (modifier == null)
                return;
            try
            {
                var modRepo = new ModifierRepository();
                modRepo.DeleteModifier(modifier.mdf_idModifier);
                Modifiers.Remove(modifier);
                MessageBox.Show("Modificador eliminado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el modificador: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al eliminar modificador: " + ex.Message);
            }
        }

        #endregion

        #region Comandos de Medios de Pago

        private void UpdateMeanPayment(GT_MeanPayment payment)
        {
            if (payment == null)
                return;
            try
            {
                var mpRepo = new MeanPaymentRepository();
                mpRepo.UpdateMeanPayment(payment);
                MessageBox.Show("Medio de pago actualizado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar el medio de pago: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al actualizar medio de pago: " + ex.Message);
            }
        }

        private void DeleteMeanPayment(GT_MeanPayment payment)
        {
            if (payment == null)
                return;
            try
            {
                var mpRepo = new MeanPaymentRepository();
                mpRepo.DeleteMeanPayment(payment.mep_idMeanPayment);
                MeanPayments.Remove(payment);
                MessageBox.Show("Medio de pago eliminado correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el medio de pago: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Error al eliminar medio de pago: " + ex.Message);
            }
        }

        #endregion
    }
}
