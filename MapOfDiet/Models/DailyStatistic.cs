using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс ежедневной статистики питания
    public class DailyStatistic
    {
        // Дата статистики
        public DateOnly Date { get; set; }

        // Целевая калорийность
        public double TargetCalories { get; set; }

        // Фактическая калорийность
        public double ActualCalories { get; set; }

        // Целевое количество белков
        public double TargetProteins { get; set; }

        // Фактическое количество белков
        public double ActualProteins { get; set; }

        // Целевое количество жиров
        public double TargetFats { get; set; }

        // Фактическое количество жиров
        public double ActualFats { get; set; }

        // Целевое количество углеводов
        public double TargetCarbohydrates { get; set; }

        // Фактическое количество углеводов
        public double ActualCarbohydrates { get; set; }
    }
}

