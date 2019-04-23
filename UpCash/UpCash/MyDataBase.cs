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
                InitializeDatabase();
                File.SetAttributes(DBName, FileAttributes.Hidden);
            }
        }

        private void InitializeDatabase()
        {
            // Создание таблицы Account
            ExecuteQueryWithoutAnswer("CREATE TABLE Account" +
                "(name TEXT NOT NULL," +
                "balance REAL NOT NULL," +
                "currency  TEXT NOT NULL," +
                "PRIMARY KEY(name)," +
                "FOREIGN KEY(currency) REFERENCES Currency(code));");

            // Создание таблицы Currency
            ExecuteQueryWithoutAnswer("CREATE TABLE Currency" +
                "(code TEXT NOT NULL," +
                "name TEXT NOT NULL," +
                "PRIMARY KEY(code));");

            // Создание таблицы Item
            ExecuteQueryWithoutAnswer("CREATE TABLE Item" +
                "(name_item TEXT NOT NULL," +
                "type_item TEXT NOT NULL," +
                "FOREIGN KEY(type_item) REFERENCES TypeItem(type_item)," +
                "PRIMARY KEY(name_item));");

            // Создание таблицы SubItem
            ExecuteQueryWithoutAnswer("CREATE TABLE SubItem" +
                "(name_sub_item TEXT NOT NULL," +
                "name_item TEXT NOT NULL," +
                "FOREIGN KEY(name_item) REFERENCES Item(name_item)," +
                "PRIMARY KEY(name_sub_item));");

            // Создание таблицы TypeItem
            ExecuteQueryWithoutAnswer("CREATE TABLE TypeItem" +
                "(type_item TEXT NOT NULL," +
                "PRIMARY KEY(type_item));");

            // Создание таблицы TypeItem
            ExecuteQueryWithoutAnswer("INSERT INTO TypeItem VALUES" +
                "('Расход')," +
                "('Доход');");
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
            _connection = new SQLiteConnection("Data Source=" + DBName + "; foreign keys=true;");
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
            string answer = _command.ExecuteScalar() as string;

            CloseConnection();

            return answer;
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
