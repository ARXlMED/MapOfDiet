using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public char Gender { get; set; }
        public double NowWeight { get; set; }
        public double TargetWeight { get; set; }
        public List<Category> LikeCategories { get; set; }
        public List<Category> DislikeCategories { get; set; }
    }
}
