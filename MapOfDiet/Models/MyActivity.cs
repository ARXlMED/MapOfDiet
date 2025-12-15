using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class MyActivity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MeasureName { get; set; }
        public double MeasureToCalories { get; set; }
        public byte[] Image { get; set; }
        public int EnteredAmount { get; set; }
    }
}
