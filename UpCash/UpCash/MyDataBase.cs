using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace UpCash
{
    internal class MyDataBase
    {
        private const string DBName = "UpCash.sqlite";
        private static MyDataBase _instance;
        private static SQLiteConnection _connection;
        private static SQLiteCommand _command;

        private MyDataBase()
        {
            if (!File.Exists(DBName))
            {
                // TODO: СОЗДАЙ ВСЕ ТАБЛИЦЫ
                File.SetAttributes(DBName, FileAttributes.Hidden);
            }
        }

        /// <summary> Возвращает объект базы данных. </summary>
        public static MyDataBase GetDB()
        {
            if (_instance == null) _instance = new MyDataBase();
            return _instance;
        }

        /// <summary> Этот метод открывает подключение БД. </summary>
        /// <remarks> Перед открытием подключения, нужно указать DBName. </remarks>
        private void OpenConnection()
        {
            _connection = new SQLiteConnection("Data Source=" + DBName);
            _command = new SQLiteCommand(_connection);
            _connection.Open();
        }

        /// <summary> Этот метод закрывает подключение к БД. </summary>
        public void CloseConnection()
        {
            _connection.Close();
            _command.Dispose();
        }

        /// <summary> Этот метод выполняет запрос query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public void ExecuteQueryWithoutAnswer(string query)
        {
            OpenConnection();

            _command.CommandText = query;
            _command.ExecuteNonQuery();

            CloseConnection();
        }

        /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
        /// <param name="query"> Собственно запрос. </param>
        /// <returns> Возвращает значение 1 строки 1 стобца, если оно имееться. </returns>
        public string ExecuteQueryWithAnswer(string query)
        {
            OpenConnection();

            _command.CommandText = query;
            object answer = _command.ExecuteScalar();

            CloseConnection();

            if (answer != null) return answer.ToString();
            else return null;
        }

        /// <summary> Этот метод возваращает таблицу, которая являеться результатом выборки запроса query. </summary>
        /// <param name="query"> Собственно запрос. </param>
        public DataTable GetTable(string query)
        {
            OpenConnection();

            DataTable table = new DataTable();
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, _connection))
            {
                adapter.Fill(table);
            }

            CloseConnection();
            return table;
        }
    }
}
