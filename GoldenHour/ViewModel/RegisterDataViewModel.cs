using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace GoldenHour.ViewModel
{
    public class RegisterDataViewModel : ViewModelBase
    {
        private ViewModelBase _currentSubView;
        public ViewModelBase CurrentSubView
        {
            get => _currentSubView;
            set { _currentSubView = value; OnPropertyChanged(nameof(CurrentSubView)); }
        }

        // Comandos para cada opción
        public ICommand ShowUsuarioSubViewCommand { get; }
        public ICommand ShowCategoriasSubViewCommand { get; }
        public ICommand ShowProductosSubViewCommand { get; }
        public ICommand ShowModificadoresSubViewCommand { get; }

        public RegisterDataViewModel()
        {
            // Inicializar los comandos
            ShowCategoriasSubViewCommand = new ViewModelCommand(_ => ExecuteShowCategoriasSubView());
            ShowProductosSubViewCommand = new ViewModelCommand(_ => ExecuteShowProductosSubView());
            ShowUsuarioSubViewCommand = new ViewModelCommand(_ => ExecuteShowUsuarioSubView());
            ShowModificadoresSubViewCommand = new ViewModelCommand(_ => ExecuteShowModificadoresSubView());
            // Puedes establecer un sub view por defecto
            ExecuteShowCategoriasSubView();
        }

        private void ExecuteShowUsuarioSubView()
        {
            
            CurrentSubView = new UsuarioSubViewModel();
        }

        private void ExecuteShowCategoriasSubView()
        {
            CurrentSubView = new CategoriasSubViewModel();
        }

        private void ExecuteShowProductosSubView()
        {
            CurrentSubView = new ProductosSubViewModel();
        }

        private void ExecuteShowModificadoresSubView()
        {
           CurrentSubView = new ModificadoresSubViewModel();
        }


    }
}
