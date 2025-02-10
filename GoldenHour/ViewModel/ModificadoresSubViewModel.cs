using GoldenHour.Model;
using GoldenHour.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoldenHour.ViewModel
{
    public class ModificadoresSubViewModel : ViewModelBase
    {// Propiedad para mostrar mensajes en la vista
        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); }
        }

        // Propiedad para el nombre del modificador
        private string _modifierName;
        public string ModifierName
        {
            get => _modifierName;
            set { _modifierName = value; OnPropertyChanged(nameof(ModifierName)); }
        }

        // Propiedad para la descripción del modificador
        private string _modifierDescription;
        public string ModifierDescription
        {
            get => _modifierDescription;
            set { _modifierDescription = value; OnPropertyChanged(nameof(ModifierDescription)); }
        }

        // Propiedad para el porcentaje del modificador
        private int? _modifierPercentage;
        public int? ModifierPercentage
        {
            get => _modifierPercentage;
            set { _modifierPercentage = value; OnPropertyChanged(nameof(ModifierPercentage)); }
        }

        // Comando para guardar el modificador
        public ICommand SaveModifierCommand { get; }

        public ModificadoresSubViewModel()
        {
            Mensaje = "Ingrese los datos del modificador";
            // Se asume que tienes implementado un ICommand (por ejemplo, ViewModelCommand o RelayCommand)
            SaveModifierCommand = new ViewModelCommand(_ => SaveModifier(), _ => !string.IsNullOrWhiteSpace(ModifierName));
        }

        private void SaveModifier()
        {
            try
            {
                var repository = new ModifierRepository();
                var newModifier = new GT_Modifier
                {
                    mdf_name = ModifierName,
                    mdf_description = ModifierDescription,
                    mdf_status = 1, // Por ejemplo, 1 = activo
                    mdf_percentage = ModifierPercentage
                };

                int result = repository.InsertModifier(newModifier);
                if (result > 0)
                {
                    Mensaje = "Modificador guardado correctamente.";
                    // Limpiar los campos para ingresar un nuevo modificador
                    ModifierName = string.Empty;
                    ModifierDescription = string.Empty;
                    ModifierPercentage = null;
                }
                else
                {
                    Mensaje = "No se pudo guardar el modificador.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al guardar modificador: " + ex.Message;
            }
        }
    }
}