using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс записи о съеденном продукте
    public class FoodRecord
    {
        // Дата и время приёма пищи
        public DateTime DateTime { get; set; }

        // Ссылка на продукт
        public Food Food { get; set; }

        // Масса съеденного продукта
        public double Mass { get; set; }

        // Дополнительное описание
        public string Description { get; set; }
    }
}

