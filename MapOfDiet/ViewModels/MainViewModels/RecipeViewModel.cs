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
        // Отвечает за то загружено ли блюдо, если нет то интерфейс скрывается до момента загрузки
        [ObservableProperty] private bool isLoaded;

        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }

        // Переменная отвечающая за поле ввода имени рецепта
        [ObservableProperty] private string searchNameRecipe = string.Empty;

        // Еда информация о которой выводится
        [ObservableProperty] private Food recipe;

        // Список найденной еды исходя из запроса поиска
        public ObservableCollection<Food> SearchResultsRecipe { get; } = new();

        // Искать рецепты по имени
        [RelayCommand]
        private void SearchRecipe()
        {
            SearchResultsRecipe.Clear();
            foreach (var food in DBWork.SearchFoodsByName(searchNameRecipe))
                SearchResultsRecipe.Add(food);
        }

        // Показать конкретный рецепт
        [RelayCommand]
        private void ShowRecipe(Food food)
        {
            if (food == null) return;
            Recipe = food;   
            IsLoaded = true; 
        }
    }
}
