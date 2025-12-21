using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс авторизации пользователя
    public class UserAuth
    {
        // Уникальный идентификатор пользователя
        public int? UserId { get; set; }

        // Логин пользователя
        public string Login { get; set; }

        // Хэш пароля
        public byte[] Hash { get; set; }

        // Соль для хэширования
        public byte[] Salt { get; set; }

        // Статус пользователя (обычный/админ)
        public bool Status { get; set; }
    }
}
