using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс записи веса пользователя
    public class WeightRecord
    {
        // Дата записи
        public DateOnly Date { get; set; }

        // Текущий вес
        public double Weight { get; set; }

        // Целевой вес
        public double TargetWeight { get; set; }
    }
}
