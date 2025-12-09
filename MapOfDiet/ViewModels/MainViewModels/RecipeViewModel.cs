using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.ViewModels
{
    public partial class RecipeViewModel : ObservableObject
    {
        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private string searchName = string.Empty;
        [ObservableProperty] private Food recipe;
        public ObservableCollection<Food> SearchResultsFoods { get; } = new();

        [RelayCommand]
        private void SearchRecipe()
        {
            SearchResultsFoods.Clear();
            foreach (var food in DBWork.SearchFoodsByName(searchName))
                SearchResultsFoods.Add(food);
        }

        [RelayCommand]
        private void OpenRecipe()
        {
            // рядом с каждым найденынм рецептом должна открываться кнопка на которую нажимаешь и тогда выбирается recipe. После этого скорее всего надо сделать переход в другую xaml с просмотром блюда
        }
    }
}
