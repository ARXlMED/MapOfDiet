using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс ингредиента
    public class Ingredient
    {
        // Уникальный идентификатор ингредиента
        public int IngrId { get; set; }

        // Название ингредиента
        public string Name { get; set; }

        // Описание ингредиента
        public string Description { get; set; }

        // Единица измерения
        public string MeasureName { get; set; }

        // Масса ингредиента
        public double Mass { get; set; }

        // Изображение ингредиента
        public byte[] Image { get; set; }
    }
}
