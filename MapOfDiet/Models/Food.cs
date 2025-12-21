using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс продукта/рецепта
    public class Food
    {
        // Уникальный идентификатор продукта
        public int FoodId { get; set; }

        // Название продукта
        public string Name { get; set; }

        // Масса продукта
        public double Mass { get; set; }

        // Калорийность продукта
        public double Calories { get; set; }

        // Количество белков
        public double Proteins { get; set; }

        // Количество жиров
        public double Fats { get; set; }

        // Количество углеводов
        public double Carbohydrates { get; set; }

        // Список категорий продукта
        public List<Category> Categories { get; set; }

        // Список ингредиентов продукта
        public List<Ingredient> Ingredients { get; set; }

        // Описание продукта
        public string Description { get; set; }

        // Описание приготовления
        public string CookingDescription { get; set; }

        // Изображение продукта
        public byte[] Image { get; set; }

        // Введённая масса продукта
        public double EnteredMass { get; set; }
    }
}
