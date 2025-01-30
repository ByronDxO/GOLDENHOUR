using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoldenHour.Model;
using GoldenHour.Repositories;
namespace GoldenHour.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        //Fields
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;

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


        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.usu_Username;
                CurrentUserAccount.IdUser = user.usu_IdUser;
                CurrentUserAccount.DisplayName = $"Bienvenido {user.usu_Firstname} {user.usu_LastName} ;)";
            }
            else
            {
                CurrentUserAccount.DisplayName = "Usuario invalido , por favor iniciar sesión";
                //Hide child views.
            }
        }

    }
}
