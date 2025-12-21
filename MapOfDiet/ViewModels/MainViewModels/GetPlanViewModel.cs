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
        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }

        // Количество калорий которые необходимо съедать
        [ObservableProperty] private double calories;
        // Количество белков которые необходимо съедать
        [ObservableProperty] private double proteins;
        // Количество жиров которые необходимо съедать
        [ObservableProperty] private double fats;
        // Количество углеводов которые необходимо съедать
        [ObservableProperty] private double carbohydrates;
        // Индекс массы тела
        [ObservableProperty] private double bmi;
        // Рекомендуемый диапозон выбора желаемого веса для данного роста
        [ObservableProperty] private string recommendedWeightRange;
        // Предупреждение если аккаунт не найден
        [ObservableProperty] private string warning;

        // Составление плана питания
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
