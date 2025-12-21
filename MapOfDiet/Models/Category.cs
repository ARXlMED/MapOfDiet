using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс категории продуктов
    public class Category
    {
        // Уникальный идентификатор категории
        public int CategoryId { get; set; }

        // Название категории
        public string Name { get; set; }

        // Описание категории
        public string Description { get; set; }

        // Флаг активности категории
        public bool IsEnabled { get; set; } = false;

        // Изображение категории
        public byte[] Image { get; set; }
    }
}
