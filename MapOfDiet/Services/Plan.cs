using MapOfDiet.Models;
using System;

namespace MapOfDiet.Services
{
    public class Plan
    {
        public double BMI { get; private set; }
        public double BMR { get; private set; }
        public double TDEE { get; private set; }
        public int NumberDays { get; private set; }
        public double CaloriesTarget { get; private set; }
        public double ProteinsDay { get; private set; }
        public double FatsDay { get; private set; }
        public double CarbohydratesDay { get; private set; }
        public (double Min, double Max) RecommendedWeight { get; private set; }
        public string WarningMessage { get; private set; }

        public Plan(UserProfile userProfile, int numberDays, double activityFactor = 1.3)
        {
            if (userProfile == null) return;

            NumberDays = numberDays;

            // BMR по формуле Миффлина-Сан Жеора
            if (userProfile.Gender == 'M')
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age + 5;
            else
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age - 161;

            BMI = userProfile.NowWeight / Math.Pow(userProfile.Height / 100.0, 2);
            TDEE = BMR * activityFactor;

            double deltaMass = userProfile.TargetWeight - userProfile.NowWeight;
            double needCaloriesAllTime = deltaMass * 7700; // 7700 ккал ≈ 1 кг
            double caloriesDeltaPerDay = needCaloriesAllTime / NumberDays;

            CaloriesTarget = TDEE + caloriesDeltaPerDay;

            ProteinsDay = 2 * userProfile.NowWeight;
            FatsDay = 0.25 * CaloriesTarget / 9;
            CarbohydratesDay = (CaloriesTarget - (ProteinsDay * 4 + FatsDay * 9)) / 4;

            // Рекомендованный вес по нормальному диапазону BMI
            double minWeight = 18.5 * Math.Pow(userProfile.Height / 100.0, 2);
            double maxWeight = 24.9 * Math.Pow(userProfile.Height / 100.0, 2);
            RecommendedWeight = (minWeight, maxWeight);

            // Проверка на безопасность
            if (Math.Abs(caloriesDeltaPerDay) > 1000)
            {
                WarningMessage = "Вы указали слишком маленькое количество дней для безопасного изменения веса. " +
                                 "Рекомендуется увеличить срок, чтобы изменение происходило постепенно.";
            }
        }
    }
}
