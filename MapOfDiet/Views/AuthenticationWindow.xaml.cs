using MapOfDiet.ViewModels;
using System.Linq;
using System.Windows;

namespace MapOfDiet
{
    public partial class AuthenticationWindow : Window
    {
        private readonly AuthenticationViewModel _vm = new AuthenticationViewModel();

        public AuthenticationWindow()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        private void Authorization_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.Authorize();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            _vm.Password = PasswordBox.Password;
            _vm.Register();
        }
        private void HyperlinkToRegistration_Click(object sender, RoutedEventArgs e)
        {
            ForAuthorization.Visibility = Visibility.Collapsed;
            ForRegistration.Visibility = Visibility.Visible;
        }

        private void HyperlinkToAuthorization_Click(object sender, RoutedEventArgs e)
        {
            ForRegistration.Visibility = Visibility.Collapsed;
            ForAuthorization.Visibility = Visibility.Visible;
        }

    }
}
