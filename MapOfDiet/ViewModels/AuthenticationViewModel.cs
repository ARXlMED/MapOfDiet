using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Services;
using System.Windows;

namespace MapOfDiet.ViewModels
{
    public partial class AuthenticationViewModel : ObservableObject
    {
        // Логин пользователя
        [ObservableProperty]
        private string login;

        // Пароль пользователя
        [ObservableProperty]
        private string password;

        // Режим регистрации (true — регистрация, false — авторизация)
        [ObservableProperty]
        private bool isRegistrationMode = false;

        // Команда авторизации пользователя
        [RelayCommand]
        public void Authorize()
        {
            int? result = AuthorizationService.authorization(Login, Password);

            if (result == null)
            {
                // Пользователь не найден
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (result == 0)
            {
                // Неверный пароль
                MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Авторизация успешна: сохраняем пользователя и открываем следующее окно
                UserSession.SetUser((int)result, Login);
                GoNextWindow();
                CloseAuthenticationWindow();
            }
        }

        // Команда регистрации нового пользователя
        [RelayCommand]
        public void Register()
        {
            bool success = AuthorizationService.registration(Login, Password);

            if (!success)
            {
                // Ошибка: логин уже существует или пароль слишком короткий
                MessageBox.Show("Такой логин уже существует или пароль состоит меньше чем из 4 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Регистрация успешна: сохраняем пользователя и открываем следующее окно
                UserSession.SetUser(DBWork.getUserId(Login), Login);
                GoNextWindow();
                CloseAuthenticationWindow();
            }
        }

        // Переключение в режим регистрации
        [RelayCommand]
        private void SwitchToRegistration() => IsRegistrationMode = true;

        // Переключение в режим авторизации
        [RelayCommand]
        private void SwitchToAuthorization() => IsRegistrationMode = false;

        // Открытие следующего окна после авторизации/регистрации
        private void GoNextWindow()
        {
            if (DBWork.checkUserStatus(UserSession.UserId) == true)
            {
                // Если пользователь администратор — открыть окно администратора
                var adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            else
            {
                // Если обычный пользователь — открыть главное окно приложения
                var mainWindow = new MapOfDiet.Views.MainWindow();
                mainWindow.Show();
            }
        }

        // Закрытие окна аутентификации
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
