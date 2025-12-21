using MapOfDiet.Models;
using System;

namespace MapOfDiet.Services
{
    public class Plan
    {
        // Индекс массы тела (BMI)
        public double BMI { get; private set; }

        // Базовый уровень метаболизма (BMR)
        public double BMR { get; private set; }

        // Общие энергозатраты организма (TDEE)
        public double TDEE { get; private set; }

        // Количество дней для достижения цели
        public int NumberDays { get; private set; }

        // Целевая калорийность в день
        public double CaloriesTarget { get; private set; }

        // Количество белков в день (граммы)
        public double ProteinsDay { get; private set; }

        // Количество жиров в день (граммы)
        public double FatsDay { get; private set; }

        // Количество углеводов в день (граммы)
        public double CarbohydratesDay { get; private set; }

        // Рекомендованный диапазон веса по нормальному BMI
        public (double Min, double Max) RecommendedWeight { get; private set; }

        // Предупреждающее сообщение о безопасности плана
        public string WarningMessage { get; private set; }

        // Конструктор плана питания и активности
        public Plan(UserProfile userProfile, int numberDays, double activityFactor = 1.3)
        {
            // Если профиль пользователя не задан — выход
            if (userProfile == null) return;

            // Устанавливаем количество дней
            NumberDays = numberDays;

            // Расчёт BMR по формуле Миффлина-Сан Жеора
            if (userProfile.Gender == 'M')
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age + 5;
            else
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age - 161;

            // Расчёт BMI (индекса массы тела)
            BMI = userProfile.NowWeight / Math.Pow(userProfile.Height / 100.0, 2);

            // Расчёт TDEE (суточных энергозатрат с учётом активности)
            TDEE = BMR * activityFactor;

            // Разница между текущим и целевым весом
            double deltaMass = userProfile.TargetWeight - userProfile.NowWeight;

            // Необходимые калории для изменения веса (7700 ккал ≈ 1 кг)
            double needCaloriesAllTime = deltaMass * 7700;

            // Суточный дефицит/профицит калорий
            double caloriesDeltaPerDay = needCaloriesAllTime / NumberDays;

            // Целевая калорийность в день
            CaloriesTarget = TDEE + caloriesDeltaPerDay;

            // Расчёт белков (2 г на кг веса)
            ProteinsDay = 2 * userProfile.NowWeight;

            // Расчёт жиров (25% калорийности, делённое на 9 ккал/г)
            FatsDay = 0.25 * CaloriesTarget / 9;

            // Расчёт углеводов (оставшиеся калории, делённые на 4 ккал/г)
            CarbohydratesDay = (CaloriesTarget - (ProteinsDay * 4 + FatsDay * 9)) / 4;

            // Рекомендованный вес по нормальному диапазону BMI (18.5–24.9)
            double minWeight = 18.5 * Math.Pow(userProfile.Height / 100.0, 2);
            double maxWeight = 24.9 * Math.Pow(userProfile.Height / 100.0, 2);
            RecommendedWeight = (minWeight, maxWeight);

            // Проверка на безопасность: если изменение калорийности слишком резкое
            if (Math.Abs(caloriesDeltaPerDay) > 1000)
            {
                WarningMessage = "Вы указали слишком маленькое количество дней для безопасного изменения веса. " +
                                 "Рекомендуется увеличить срок, чтобы изменение происходило постепенно.";
            }
        }
    }
}
