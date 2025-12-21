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
    public partial class StatisticsViewModel : ObservableObject
    {
        // Вспомогательные переменные для хранения информации о иконке в TabControl
        public string Title { get; set; }
        public string IconPath { get; set; }

        // Список данных за последние 7 дней
        public ObservableCollection<DailyStatistic> Last7Days { get; } = new();

        // Функция для вызова из View
        [RelayCommand]
        public async Task RefreshAsync()
        {
            await LoadStatisticsAsync(UserSession.UserId);
        }

        // Загрузка статистики за последние 7 дней
        public async Task LoadStatisticsAsync(int userId)
        {
            Last7Days.Clear();

            var target = await CalculateTargetCaloriesAsync(userId, DateOnly.FromDateTime(DateTime.Today));

            for (int i = 6; i >= 0; i--)
            {
                var date = DateOnly.FromDateTime(DateTime.Today.AddDays(-i));
                var dateTime = date.ToDateTime(TimeOnly.MinValue);

                double actual = await DBWork.GetActualCaloriesAsync(userId, dateTime);
                double actualActivity = await DBWork.GetActualCaloriesActivityAsync(userId, dateTime);

                Last7Days.Add(new DailyStatistic
                {
                    Date = date,
                    TargetCalories = target,
                    ActualCalories = actual - actualActivity
                });
            }
        }

        // Подсчёт требуемых калорий для данного человека
        private async Task<double> CalculateTargetCaloriesAsync(int userId, DateOnly date)
        {
            var user = await DBWork.GetUserProfileAsync(userId); 
            if (user == null) return 0;

            var plan = new Plan(user, 180);
            return plan.CaloriesTarget;
        }
    }
}
