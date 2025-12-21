using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MapOfDiet.ViewModels;

namespace MapOfDiet.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // Вкладки или модули
        public ObservableCollection<object> Tabs { get; }

        // Создание вкладок
        public MainViewModel()
        {
            Tabs = new ObservableCollection<object>()
            {
                new ProfileViewModel { Title = "Профиль", IconPath = "/Images/TabControl/Profile.png" },
                new RecipeViewModel { Title = "Рецепты", IconPath = "/Images/TabControl/Recipe_book_v2.png" },
                new AddFoodRecordViewModel { Title = "Еда", IconPath = "/Images/TabControl/Add_food.png" },
                new AddActivityRecordViewModel { Title = "Активность", IconPath = "/Images/TabControl/Add_physical_activity.png" },
                new StatisticsViewModel { Title = "Статистика", IconPath = "/Images/TabControl/Statistics.png" },
                new GetPlanViewModel { Title = "План", IconPath = "/Images/TabControl/Meal_Plan.png" }
            };
        }
    }
}
