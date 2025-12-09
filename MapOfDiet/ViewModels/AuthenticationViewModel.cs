using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Services;
using System.Windows;

namespace MapOfDiet.ViewModels
{
    public partial class AuthenticationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isRegistrationMode = false;

        [RelayCommand]
        public void Authorize()
        {
            int? result = AuthorizationService.authorization(Login, Password);

            if (result == null)
            {
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (result == 0)
            {
                MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UserSession.SetUser((int)result, Login);
                GoNextWindow();
                CloseAuthenticationWindow();
            }
        }

        [RelayCommand]
        public void Register()
        {
            bool success = AuthorizationService.registration(Login, Password);

            if (!success)
            {
                MessageBox.Show("Такой логин уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UserSession.SetUser(DBWork.getUserId(Login), Login);
                GoNextWindow();
                CloseAuthenticationWindow();
            }
        }

        [RelayCommand]
        private void SwitchToRegistration() => IsRegistrationMode = true;

        [RelayCommand]
        private void SwitchToAuthorization() => IsRegistrationMode = false;

        private void GoNextWindow()
        {
            if (DBWork.checkUserStatus(UserSession.UserId) == true)
            {
                var adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            else
            {
                
                var mainWindow = new MapOfDiet.Views.MainWindow();
                mainWindow.Show();
            }
        }

        private void CloseAuthenticationWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AuthenticationWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
