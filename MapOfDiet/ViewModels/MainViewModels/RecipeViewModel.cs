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
        [ObservableProperty] private bool isLoaded;

        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private string searchNameRecipe = string.Empty;

        [ObservableProperty] private Food recipe;
        public ObservableCollection<Food> SearchResultsRecipe { get; } = new();

        [RelayCommand]
        private void SearchRecipe()
        {
            SearchResultsRecipe.Clear();
            foreach (var food in DBWork.SearchFoodsByName(searchNameRecipe))
                SearchResultsRecipe.Add(food);
        }

        [RelayCommand]
        private void ShowRecipe()
        {

        }
    }
}
