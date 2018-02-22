using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace StudentskiDom
{
    class Database
    {
        public SQLiteConnection myConnection;

        public Database ()
        {
            myConnection = new SQLiteConnection("Data Source = database.db");
            if (!File.Exists("./database.db"))
            {
                createDB();
            }
        }

        private static void createDB()
        {
            SQLiteConnection.CreateFile("database.db");
        }
    }
}
