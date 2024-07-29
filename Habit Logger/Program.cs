using System;
using System.Data.SQLite;

namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habittracker.db; Version=3";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            GetUserInput();
        }

        private static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        private static void GetAllRecords()
        {
            string connectionString = @"Data Source=habittracker.db; Version=3";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM drinking_water", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Date: {reader["Date"]}, Quantity: {reader["Quantity"]}");
                    }
                }
                connection.Close();
            }
            Console.ReadLine();
        }

        private static void Insert()
        {
            string date = GetDateInput();
            if (date == "0") return; // Return to main menu if user entered '0'

            Console.WriteLine("\n\nPlease enter the quantity:");
            string quantityInput = Console.ReadLine();
            int quantity;
            if (!int.TryParse(quantityInput, out quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a valid number.");
                return;
            }

            string connectionString = @"Data Source=habittracker.db; Version=3";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO drinking_water (Date, Quantity) VALUES (@Date, @Quantity)", connection);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("Record inserted successfully.");
        }

        private static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu");
            string dateInput = Console.ReadLine();
            return dateInput == "0" ? "0" : dateInput;
        }

        private static void Delete()
        {
            Console.WriteLine("\n\nEnter the ID of the record you wish to delete:");
            string idInput = Console.ReadLine();
            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Invalid ID. Please enter a valid number.");
                return;
            }

            string connectionString = @"Data Source=habittracker.db; Version=3";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("DELETE FROM drinking_water WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                int affectedRows = command.ExecuteNonQuery();
                connection.Close();

                if (affectedRows > 0)
                    Console.WriteLine("Record deleted successfully.");
                else
                    Console.WriteLine("No record found with that ID.");
            }
        }

        private static void Update()
        {
            Console.WriteLine("\n\nEnter the ID of the record you wish to update:");
            string idInput = Console.ReadLine();
            int id;
            if (!int.TryParse(idInput, out id))
            {
                Console.WriteLine("Invalid ID. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter the new quantity:");
            string quantityInput = Console.ReadLine();
            int quantity;
            if (!int.TryParse(quantityInput, out quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a valid number.");
                return;
            }

            string connectionString = @"Data Source=habittracker.db; Version=3";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE drinking_water SET Quantity = @Quantity WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("Record updated successfully.");
        }
    }
}
