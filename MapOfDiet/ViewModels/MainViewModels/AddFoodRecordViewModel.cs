using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.ObjectModel;

namespace MapOfDiet.ViewModels
{
    public partial class AddFoodRecordViewModel : ObservableObject
    {
        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }

        // Переменная отвечающая за поле ввода имени пищи
        [ObservableProperty] private string searchNameFood;

        // Список пищи найденной в результате поиска
        public ObservableCollection<Food> SearchResultsFood { get; } = new();

        // Дата приёма пищи
        [ObservableProperty] private DateTime dateFood = DateTime.Now.Date;
        // Время приёма пищи
        [ObservableProperty] private DateTime timeFood = DateTime.Now;

        // Ищет пищу по заданному имени и добавляет их в список активностей
        [RelayCommand]
        private void SearchFood()
        {
            SearchResultsFood.Clear();
            foreach (var food in DBWork.SearchFoodsByName(SearchNameFood))
                SearchResultsFood.Add(food);
        }

        // Добавляет запись о приёме пищи данного пользователя 
        [RelayCommand]
        private void AddFood(Food food)
        {
            if (food == null) return;

            DateTime mealDateTime = new DateTime(DateFood.Year, DateFood.Month, DateFood.Day, TimeFood.Hour, TimeFood.Minute, TimeFood.Second);

            var foodRecord = new FoodRecord
            {
                Food = food,               
                DateTime = mealDateTime,   
                Mass = food.EnteredMass,        
                Description = food.Name    
            };

            DBWork.PushFoodRecord(foodRecord);
            food.EnteredMass = 0;
        }
    }
}
