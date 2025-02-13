using GoldenHour.Model;
using GoldenHour.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.ViewModel
{
    public class ConsultaDatosViewModel : ViewModelBase
    {
        // Propiedades para cada conjunto de datos
        public ObservableCollection<GT_Receipt> Receipts { get; set; }
        public ObservableCollection<UserAccountModel> Employees { get; set; } // Asumiendo que tienes un modelo para empleados
        public ObservableCollection<GT_Product> Products { get; set; }
        public ObservableCollection<GT_Category> Categories { get; set; }
        public ObservableCollection<GT_Modifier> Modifiers { get; set; }
        public ObservableCollection<GT_MeanPayment> MeanPayments { get; set; }

        public ConsultaDatosViewModel()
        {
            Receipts = new ObservableCollection<GT_Receipt>();
            Employees = new ObservableCollection<UserAccountModel>();
            Products = new ObservableCollection<GT_Product>();
            Categories = new ObservableCollection<GT_Category>();
            Modifiers = new ObservableCollection<GT_Modifier>();
            MeanPayments = new ObservableCollection<GT_MeanPayment>();

            // Cargar los datos
            LoadReceipts();
            LoadEmployees();
            LoadProducts();
            LoadCategories();
            LoadModifiers();
            LoadMeanPayments();
        }

        private void LoadReceipts()
        {
            try
            {
                var receiptRepo = new ReceiptRepository();
                var list = receiptRepo.GetReceipts(); // Asume que creas un método GetReceipts() en tu repository
                Receipts.Clear();
                foreach (var rec in list)
                {
                    Receipts.Add(rec);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Error cargando recibos: " + ex.Message);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                var userRepo = new UserRepository();
                var list = userRepo.GetAllEmployees(); // Esto retorna IEnumerable<GT_User>
                Employees.Clear();
                foreach (var emp in list)
                {
                    // Mapea GT_User a UserAccountModel
                    Employees.Add(new UserAccountModel
                    {
                        IdUser = emp.usu_idUser,
                        Username = emp.usu_username,
                        DisplayName = $"{emp.usu_firstname} {emp.usu_lastName}"
                        // Puedes asignar otras propiedades según la definición de UserAccountModel
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
                var list = prodRepo.GetAllProducts(); // Asume que creas un método GetAllProducts() en el repository
                Products.Clear();
                foreach (var prod in list)
                {
                    Products.Add(prod);
                }
            }
            catch (System.Exception ex)
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
                {
                    Categories.Add(cat);
                }
            }
            catch (System.Exception ex)
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
                {
                    Modifiers.Add(mod);
                }
            }
            catch (System.Exception ex)
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
                {
                    MeanPayments.Add(mp);
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Error cargando medios de pago: " + ex.Message);
            }
        }
    }
}
