using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public List<Category> Categories { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string Description { get; set; } 
    }
}
