using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Models
{
    // Класс записи о выполненной активности
    public class MyActivityRecord
    {
        // Дата и время выполнения
        public DateTime DateTime { get; set; }

        // Ссылка на активность
        public MyActivity Activity { get; set; }

        // Количество выполненной активности
        public int Amount { get; set; }

        // Дополнительное описание
        public string Description { get; set; }
    }
}

