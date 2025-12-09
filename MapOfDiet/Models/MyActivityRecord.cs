using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    class MyActivityRecord
    {
        public DateTime DateTime { get; set; }
        public MyActivity Activity { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
