using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;


namespace sem1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ctrl + f5
            // Console.Title = "titluu";
            // Console.ForegroundColor = ConsoleColor.Blue;
            // Console.BackgroundColor = ConsoleColor.White;
            // Console.Clear();
            // Console.WriteLine("heeeeello!!");

            try
            {
                string connectionString = @"Server = DESKTOP-TQEKTP3\SQLEXPRESS; Initial Catalog = S1SGBD; Integrated Security = true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Console.WriteLine("Starea conexiunii: {0}", connection.State);
                    connection.Open();
                    Console.WriteLine("Starea conexiunii dupa apelul metodei open: {0}", connection.State);

                    SqlCommand insertCommand = new SqlCommand("INSERT INTO Cadouri(descriere, valoare, posesor)" +
                        "VALUES (@descriere, @valoare, @posesor);", connection);
                    insertCommand.Parameters.AddWithValue("@descriere", "urs adevarat");
                    insertCommand.Parameters.AddWithValue("@valoare", 30000.0F);
                    insertCommand.Parameters.AddWithValue("@posesor", "Maria");
                    int numberofrowsaffectedbyinsert = insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Numarul de inregistrari afectate de insert este {0}", numberofrowsaffectedbyinsert);

                    SqlCommand updateCommand = new SqlCommand("UPDATE Cadouri SET valoare = @valoareNoua WHERE posesor = @posesor;", connection);
                    updateCommand.Parameters.AddWithValue("@valoareNoua", 45000.0F);
                    updateCommand.Parameters.AddWithValue("@posesor", "Maria");
                    updateCommand.ExecuteNonQuery();

                    SqlCommand selectCommand = new SqlCommand("SELECT descriere, valoare, posesor FROM Cadouri;", connection);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Afisarea datelor din tabelul Cadouri");
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}\t{1}\t{2}", reader.GetString(0), reader.GetFloat(1), reader.GetString(2));
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Mesajul mesasjul {0}", ex.Message);
            }
        }
    }
}
