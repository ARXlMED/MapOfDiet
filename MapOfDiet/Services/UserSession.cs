using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Services
{
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static string Login { get; private set; }

        public static void SetUser(int userId, string login)
        {
            UserId = userId;
            Login = login;
        }

        public static void Clear()
        {
            UserId = 0;
            Login = null;
        }
    }
}

