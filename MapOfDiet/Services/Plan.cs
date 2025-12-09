using MapOfDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Services
{
    public class Plan
    {
        public double BMI; // индекс массы тела (пока что не нужен только при реализации выбора необходимого веса не самому, а автоматически)
        public double BMR; // базовый обмен веществ
        public double TDEE; // калории для поддержания веса
        public int NumberDays = 180; // количество дней для похудения
        public double CaloriesTarget; // нужно употребить калорий за день
        public double ProteinsDay; // нужно употребить белков за день
        public double FatsDay; // нужно употребить жиров за день
        public double CarbohydratesDay; // нужно употребить углеводов за день
        public Plan (UserProfile userProfile, int numberDays, double activityFactor = 1.3) 
        {
            if (userProfile == null) { return; }
            if (userProfile.Gender == 'M')
            {
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age + 5;
            }
            else
            {
                BMR = 10 * userProfile.NowWeight + 6.25 * userProfile.Height - 5 * userProfile.Age - 161;
            }

            BMI = userProfile.NowWeight / Math.Pow(userProfile.Height / 100.0, 2);
            TDEE = BMR * activityFactor;

            double deltaMass = userProfile.TargetWeight - userProfile.NowWeight;

            double needCaloriesAllTime = deltaMass * 7700;

            double caloriesDeltaPerDay = needCaloriesAllTime / NumberDays;

            CaloriesTarget = TDEE + caloriesDeltaPerDay;

            ProteinsDay = 2 * userProfile.NowWeight;
            FatsDay = 0.25 * CaloriesTarget / 9;
            CarbohydratesDay = (CaloriesTarget - (ProteinsDay * 4 + FatsDay * 9)) / 4;
        }
    }
}
