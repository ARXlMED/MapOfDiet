using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class WeightRecord
    {
        public DateOnly Date { get; set; }
        public double Weight { get; set; }
        public double TargetWeight { get; set; }
    }
}
