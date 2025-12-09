using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{

    public class UserAuth
    {
        public int? UserId { get; set; }
        public string Login { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public bool Status { get; set; }
    }
}
