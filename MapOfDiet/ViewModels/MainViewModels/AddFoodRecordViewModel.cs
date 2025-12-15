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
        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private string searchNameFood;

        public ObservableCollection<Food> SearchResultsFood { get; } = new();

        [ObservableProperty] private DateTime dateFood = DateTime.Now.Date;
        [ObservableProperty] private DateTime timeFood = DateTime.Now;

        [RelayCommand]
        private void SearchFood()
        {
            SearchResultsFood.Clear();
            foreach (var food in DBWork.SearchFoodsByName(SearchNameFood))
                SearchResultsFood.Add(food);
        }

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
