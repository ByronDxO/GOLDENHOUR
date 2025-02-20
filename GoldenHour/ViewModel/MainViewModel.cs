﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FontAwesome.Sharp;
using GoldenHour.Model;
using GoldenHour.Repositories;
namespace GoldenHour.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        //Fields
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }

        public ICommand ShowRegisterDataViewCommand { get; }




        public ICommand ShowGenerateReceiptViewCommand { get; }
        public ICommand ShowConsultaDatosViewCommand { get; }
        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            //Initialize commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowRegisterDataViewCommand = new ViewModelCommand(ExecuteShowRegisterDataViewCommand);

            ShowGenerateReceiptViewCommand = new ViewModelCommand(ExecuteShowGenerateReceiptViewCommand);
            ShowConsultaDatosViewCommand = new ViewModelCommand(ExecuteShowConsultaDatosViewCommand);
            //Default view
            ExecuteShowHomeViewCommand(null);
            LoadCurrentUserData();
        }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Ver Datos";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowConsultaDatosViewCommand(object obj)
        {
            CurrentChildView = new ConsultaDatosViewModel();
            Caption = "Ver Datos";
            Icon = IconChar.BookOpenReader;
        }
        private void ExecuteShowGenerateReceiptViewCommand(object obj)
        {
            CurrentChildView = new GenerateReceiptViewModel();
            Caption = "Generar Recibo";
            Icon = IconChar.Receipt;
        }


        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Menu Principal";
            Icon = IconChar.Home;
        }

        private void ExecuteShowRegisterDataViewCommand(object obj)
        {
            CurrentChildView = new RegisterDataViewModel();
            Caption = "Menu Ingresar Datos";
            Icon = IconChar.BookOpenReader;
        }


        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.usu_Username;
                CurrentUserAccount.IdUser = user.usu_IdUser;
                CurrentUserAccount.DisplayName = $"{user.usu_Firstname} {user.usu_LastName} ;)";
            }
            else
            {
                CurrentUserAccount.DisplayName = "Usuario invalido , por favor iniciar sesión";
                //Hide child views.
            }
        }

    }
}
