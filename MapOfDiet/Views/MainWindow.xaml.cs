using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Security.Cryptography;
using Npgsql;
using MapOfDiet.Services;

namespace MapOfDiet.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                var selectedTab = tabControl.SelectedItem as TabItem;
                if (selectedTab == null) return;

                switch (selectedTab.Name)
                {
                    case "ItemProfile":

                        break;
                    case "ItemRecipes":
                        break;
                    case "ItemAddFood":
                        break;
                    case "ItemAddPhysicalActivity":
                        break;
                    case "ItemStatistics":
                        break;
                    case "ItemPlan":
                        break;
                }
            }
        }

        // Работа с профилем

        //private void SaveAllProfile_Click(object sender, RoutedEventArgs e)
        //{
        //    // потом переделать все вводы цифр в списки или что то подобное чтобы не было ошибок преобразования
        //    string name = EnteringNameBox.Text;
        //    if (!int.TryParse(EnteringAge.Text, out int age))
        //    {
        //        MessageBox.Show("В поле возраста выбрано не целое число");
        //        return;
        //    }
        //    if (!int.TryParse(EnteringHeight.Text, out int height))
        //    {
        //        MessageBox.Show("В поле роста выбрано не целое число");
        //        return;
        //    }
        //    if (!double.TryParse(EnteringWeightBox.Text, out double weight))
        //    {
        //        MessageBox.Show("В поле веса выбрано не число");
        //        return;
        //    }
        //    if (!double.TryParse(EnteringGoalWeight.Text, out double goalWeight))
        //    {
        //        MessageBox.Show("В поле целевого веса выбрано не число");
        //        return;
        //    }
        //}
    }
}