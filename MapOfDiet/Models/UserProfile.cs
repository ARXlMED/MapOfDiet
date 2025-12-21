using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс профиля пользователя
    public class UserProfile
    {
        // Уникальный идентификатор пользователя
        public int UserId { get; set; }

        // Имя пользователя
        public string Name { get; set; }

        // Возраст пользователя
        public int Age { get; set; }

        // Рост пользователя (см)
        public int Height { get; set; }

        // Пол пользователя ('M' или 'F')
        public char Gender { get; set; }

        // Текущий вес пользователя
        public double NowWeight { get; set; }

        // Целевой вес пользователя
        public double TargetWeight { get; set; }

        // Список любимых категорий
        public List<Category> LikeCategories { get; set; }

        // Список нелюбимых категорий
        public List<Category> DislikeCategories { get; set; }
    }
}
