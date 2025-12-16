using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MapOfDiet.Models;
using MapOfDiet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.ViewModels
{
    public partial class GetPlanViewModel : ObservableObject
    {
        public string Title { get; set; }
        public string IconPath { get; set; }

        [ObservableProperty] private double calories;
        [ObservableProperty] private double proteins;
        [ObservableProperty] private double fats;
        [ObservableProperty] private double carbohydrates;
        [ObservableProperty] private double bmi;
        [ObservableProperty] private string recommendedWeightRange;
        [ObservableProperty] private string warning;

        [RelayCommand]
        public async Task LoadPlanAsync()
        {
            int userId = UserSession.UserId;
            var user = await DBWork.GetUserProfileAsync(userId);
            if (user == null)
            {
                Warning = "Не удалось загрузить профиль пользователя.";
                return;
            }

            var plan = new Plan(user, 180);

            Calories = plan.CaloriesTarget;
            Proteins = plan.ProteinsDay;
            Fats = plan.FatsDay;
            Carbohydrates = plan.CarbohydratesDay;
            Bmi = plan.BMI;
            RecommendedWeightRange = $"{plan.RecommendedWeight.Min:F1}–{plan.RecommendedWeight.Max:F1} кг";
            Warning = plan.WarningMessage;
        }
    }
}
