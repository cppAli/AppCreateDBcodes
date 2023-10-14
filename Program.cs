using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace AppCreateDB_200823
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите название базы данных:");
            string dbName = Console.ReadLine();

            // Строка подключения к SQL Server
            string connectionString = @"Data Source = DESKTOP-2T5NU5R; Initial Catalog = master; Trusted_Connection = True; TrustServerCertificate = True";

            // Создание базы данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine("Подключение открыто");
                //создать переменную отвечающая за команды
                SqlCommand command = new SqlCommand();

                string createDbQuery = $"CREATE DATABASE {dbName}";
                command.CommandText = createDbQuery;

                //определить используемое подключение
                command.Connection = connection;

                await command.ExecuteNonQueryAsync();
                Console.WriteLine("Создана База");
            }



            Console.WriteLine("Введите название таблицы:");
            string tableName = Console.ReadLine();

            Console.WriteLine("Введите поля таблицы (название и тип данных, разделенные запятой; например, Id INT, Name NVARCHAR(100)):");
            string fieldsInput = Console.ReadLine();
            string[] fields = fieldsInput.Split(',');

            string createTableQuery = $"CREATE TABLE {tableName} ({string.Join(", ", fields)})";

            // Строка подключения к созданной базе данных
            connectionString = $"Data Source = DESKTOP-2T5NU5R; Initial Catalog = {dbName}; Trusted_Connection = True; TrustServerCertificate = True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand();
                command.CommandText = createTableQuery;
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Таблица {tableName} создана с полями: {fieldsInput}");
                Console.WriteLine();
            }

            while (true)
            {
                // Вставка данных в таблицу
                
                Console.WriteLine($"Введите данные для вставки в таблицу {tableName} (значения разделены запятой; например, 1, 'Имя'): или exit для выхода");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    break;
                }

                string[] values = input.Split(',');
                if (values.Length != fields.Length)
                {
                    Console.WriteLine("Количество значений не соответствует количеству полей таблицы.");
                    continue;
                }

                // Подготовка SQL-запроса для вставки данных
                string insertDataQuery = $"INSERT INTO {tableName} VALUES ({string.Join(", ", values)})";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand();
                    command.CommandText = insertDataQuery;
                    command.Connection = connection;
                    await command.ExecuteNonQueryAsync();

                    Console.WriteLine($"Данные вставлены успешно в таблицу {tableName}");
                }
        
            }
        }

    }
}
