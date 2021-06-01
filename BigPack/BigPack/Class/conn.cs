using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using BigPack.Class;
using BigPack.Controls;

namespace BigPack.Class
{
    class Conn
    {
        public static string String { get; } = "Data Source = WSR; Initial Catalog = user2; User = user2; Password = passuser2"; // Подключение к бд
    }
}
