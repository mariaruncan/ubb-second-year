using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tema4
{
    public class Program
    {
        private static string connectionString = @"Server=DESKTOP-TQEKTP3\SQLEXPRESS;Database=Magazin;Integrated Security=true;";
        private static int noOfRetriesTh1 = 0;
        private static int noOfRetriesTh2 = 0;

        static void Main(string[] args)
        {
            Thread th1 = new Thread(startThread1);
            Thread th2 = new Thread(startThread2);

            th1.Start();
            th2.Start();
        }

        private static void startThread1()
        {
            Console.WriteLine("In thread 1");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("startThread1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Exit thread 1");
                }
            }
            catch(SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    Console.WriteLine("Deadlock victim thread 1");
                    noOfRetriesTh1++;
                    if (noOfRetriesTh1 == 3)
                        Console.WriteLine("Thread 1 command aborted.");
                    else
                    {
                        Console.WriteLine("Retry startThread1");
                        startThread1();
                    }
                }
                else
                    Console.WriteLine("Database error th1: " + ex.Message);
            }
        }

        private static void startThread2()
        {
            Console.WriteLine("In thread 2");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("startThread2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Exit thread 2");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    Console.WriteLine("Deadlock victim thread 2");
                    noOfRetriesTh2++;
                    if (noOfRetriesTh2 == 3)
                        Console.WriteLine("Thread 2 command aborted.");
                    else
                    {
                        Console.WriteLine("Retry startThread2");
                        startThread2();
                    }
                }
                else
                    Console.WriteLine("Database error th2: " + ex.Message);
            }
        }
    }
}
