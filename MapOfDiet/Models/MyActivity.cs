using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс физической активности
    public class MyActivity
    {
        // Уникальный идентификатор активности
        public int ActivityId { get; set; }

        // Название активности
        public string Name { get; set; }

        // Описание активности
        public string Description { get; set; }

        // Единица измерения активности
        public string MeasureName { get; set; }

        // Количество калорий на единицу активности
        public double MeasureToCalories { get; set; }

        // Изображение активности
        public byte[] Image { get; set; }

        // Введённое количество выполненной активности
        public int EnteredAmount { get; set; }
    }
}
