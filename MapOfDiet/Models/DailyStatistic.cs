using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class DailyStatistic
    {
        public DateOnly Date { get; set; }
        public double TargetCalories { get; set; }   
        public double ActualCalories { get; set; } 
        public double TargetProteins { get; set; }
        public double ActualProteins { get; set; }
        public double TargetFats { get; set; }
        public double ActualFats { get; set; }
        public double TargetCarbohydrates { get; set; }
        public double ActualCarbohydrates { get; set; }
    }

}
