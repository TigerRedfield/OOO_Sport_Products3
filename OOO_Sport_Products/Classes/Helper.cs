using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOO_Sport_Products.Classes
{
    public class Helper
    {
        //доступное свойство связи с БД
        public static Model.DBSportProduct DB { get; set; }
        //Доступное свойство пользователя системы
        public static Model.User user { get; set; }
    }
}
