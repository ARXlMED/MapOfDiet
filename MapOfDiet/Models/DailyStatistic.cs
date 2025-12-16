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
    }

}
