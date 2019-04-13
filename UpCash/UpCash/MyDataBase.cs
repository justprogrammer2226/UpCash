using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpCash
{

    internal static class MyDataBase
    {
        private static SQLiteConnection connection;
        private static SQLiteCommand command;

        public const string DBName = "UpCash.sqlite";

        static MyDataBase()
        {
            if (!File.Exists(DBName))
            {
                // TODO: СОЗДАЙ ВСЕ ТАБЛИЦЫ
                File.SetAttributes(DBName, FileAttributes.Hidden);
            }
        }

        /// <summary> Этот метод открывает подключение БД. </summary>
        /// <remarks> Перед открытием подключения, нужно указать DBName. </remarks>
        private static void OpenConnection()
        {
            connection = new SQLiteConnection("Data Source=" + DBName);
            command = new SQLiteCommand(connection);
            connection.Open();
        }

        /// <summary> Этот метод закрывает подключение к БД. </summary>
        public static void CloseConnection()
        {
            connection.Close();
            command.Dispose();
        }

        /// <summary> Этот метод выполняет запрос query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public static void ExecuteQueryWithoutAnswer(string query)
        {
            OpenConnection();

            command.CommandText = query;
            command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
        /// <param name="query"> Собственно запрос. </param>
        /// <returns> Возвращает значение 1 строки 1 стобца, если оно имееться. </returns>
        public static string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();

            command.CommandText = query;
            var answer = command.ExecuteScalar().ToString();

            CloseConnection();

            if (answer != null) return answer.ToString();
            else return null;
        }

        /// <summary> Этот метод возваращает таблицу, которая являеться результатом выборки запроса query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public static DataTable GetTable(string query)
        {
            OpenConnection();

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);

            DataSet DS = new DataSet();
            adapter.Fill(DS);
            adapter.Dispose();

            CloseConnection();

            return DS.Tables[0];
        }
    }
}
