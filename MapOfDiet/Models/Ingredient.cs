using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class Ingredient
    {
        public int IngrId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeasureName { get; set; }
        public double Mass { get; set; }
    }
}
