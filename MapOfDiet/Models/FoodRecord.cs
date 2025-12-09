using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class FoodRecord
    {
        public DateTime DateTime { get; set; }
        public Food Food { get; set; }
        public double Mass { get; set; }
        public string Description { get; set; }
    }
}
