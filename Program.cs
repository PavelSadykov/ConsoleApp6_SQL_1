using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Xml.Linq;
using System.IO;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)

        {
            // Создание соединения с базой данных

            string connectionString002 = @"Data Source=(localdb)\MSSQLLocalDB;
                                        Initial Catalog=master;Integrated Security=True;
                                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;
                                        ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection connection = new SqlConnection(connectionString002);
            try
            {
                // Открытие соединения с базой данных
                connection.Open();
                Console.WriteLine("Connection opened successfully.");




                // Создание SQL-запроса на добавление записи в таблицу
                string query = "INSERT INTO MyTable (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";

                // Создание объекта Command
                SqlCommand command = new SqlCommand(query, connection);


                // создаем массив для хранения информации о пользователях
                var users = new List<User>();

                // запрашиваем данные у пользователя
                Console.WriteLine("Введите данные пользователя (имя, email, телефон), разделенные пробелами:");
                Console.WriteLine("Формат номера телефона: +7XXXXXXXXXX или 8XXXXXXXXXX");
                Console.WriteLine("Для завершения ввода введите END_INPUT");

                string input;
                while ((input = Console.ReadLine()) != "END_INPUT")
                {
                    // проверяем, что ввод соответствует формату: имя email телефон
                    var match = Regex.Match(input, @"^(\S+)\s+(\S+)\s+([+]?[78]\d{10})$");
                    if (match.Success)
                    {
                        // если формат верный, добавляем пользователя в список
                        var user = new User()
                        {
                            Name = match.Groups[1].Value,
                            Email = match.Groups[2].Value,
                            Phone = match.Groups[3].Value,
                        };
                        users.Add(user);
                    }
                    else
                    {
                        // если формат неверный, выводим сообщение об ошибке
                        Console.WriteLine("Неверный формат ввода. Введите данные в формате: имя email телефон");
                    }
                }

                // выводим результаты в виде таблицы
                Console.WriteLine("\nИмя\tEmail\t\t\tТелефон");
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Name}\t{user.Email}\t{user.Phone}");

                    {


                        //SqlCommand command = new SqlCommand("INSERT INTO Customers (Name, Email, Phone) VALUES (@Name, @Email, @Phone)", connection);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Phone", user.Phone);



                        // Выполнение SQL-запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        // Закрытие соединения с базой данных

                        connection.Close();
                        Console.WriteLine("Connection closed.");

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while opening connection: " + ex.Message);
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();

        }
    }
}

/// класс, представляющий пользователя
class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}