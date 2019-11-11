using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccupayFingerPrintApp.Repo
{
    public class DBConnection
    {
        private static string connectionString = File.ReadAllText("C:/ConnectionString/ConnectionStringAccupayFp.txt");

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }


        public MySqlCommand GetCommand(string commandText, MySqlConnection connection)
        {
            return new MySqlCommand(commandText, connection);
        }

       
        public List<T> GetList<T>(MySqlDataReader reader, T obj)
        {
            var tList = new List<T>();
            while(reader.Read())
            {
                var createdObj = CreateObjectFromReader(reader, obj);
                tList.Add(createdObj);
            }

            return tList;
        } 

        public T CreateObjectFromReader<T>(MySqlDataReader reader, T obj)
        {
            int i = 0;
            Type t = obj.GetType();
            foreach (var propInfo in t.GetProperties())
            {
                propInfo.SetValue(obj, reader[i++], null);
                if (i == reader.FieldCount-1)
                {
                    break;
                }
            }

            return obj;
        }
    }
}
