using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Services
{
    public static class UserSession
    {
        // user_id пользователя
        public static int UserId { get; private set; }
        // login пользователя
        public static string Login { get; private set; }

        // Установить данные о пользователе
        public static void SetUser(int userId, string login)
        {
            UserId = userId;
            Login = login;
        }

        // Удалить данные о пользователе
        public static void Clear()
        {
            UserId = 0;
            Login = null;
        }
    }
}

