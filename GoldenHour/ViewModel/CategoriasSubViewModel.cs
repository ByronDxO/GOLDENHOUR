using GoldenHour.Model;
using GoldenHour.Repositories;
using System;
using System.Windows.Input;

namespace GoldenHour.ViewModel
{
    public class CategoriasSubViewModel : ViewModelBase
    {
        // Propiedad para mostrar mensajes en la vista
        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); }
        }

        // Propiedad para el nombre de la categoría
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(nameof(CategoryName)); }
        }

        // Propiedad para la descripción de la categoría
        private string _categoryDescription;
        public string CategoryDescription
        {
            get => _categoryDescription;
            set { _categoryDescription = value; OnPropertyChanged(nameof(CategoryDescription)); }
        }

        // Comando para guardar la categoría
        public ICommand SaveCategoryCommand { get; }

        public CategoriasSubViewModel()
        {
            // Inicialización de propiedades
            Mensaje = "Listado de Categorías";
            // Se inicializa el comando. Se asume que tienes una implementación de ICommand llamada ViewModelCommand.
            SaveCategoryCommand = new ViewModelCommand(_ => SaveCategory(), _ => !string.IsNullOrWhiteSpace(CategoryName));
        }

        private void SaveCategory()
        {
            try
            {
                var repository = new CategoryRepository();
                var newCategory = new GT_Category
                {
                    cat_name = CategoryName,
                    cat_description = CategoryDescription,
                    cat_status = 1, // Por ejemplo, 1 = activo.
                    cat_date = DateTime.Now
                };

                int result = repository.InsertCategory(newCategory);
                if (result > 0)
                {
                    Mensaje = "Categoría guardada correctamente.";
                    // Limpia los campos para permitir el ingreso de otra categoría.
                    CategoryName = string.Empty;
                    CategoryDescription = string.Empty;
                }
                else
                {
                    Mensaje = "No se pudo guardar la categoría.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al guardar categoría: " + ex.Message;
            }
        }
    }
}
