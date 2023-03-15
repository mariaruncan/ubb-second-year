using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Laborator_1_226_1_SGBD
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        string connectionString = @"Server=DESKTOP-TQEKTP3\SQLEXPRESS;Database=Lab1SGBD;Integrated Security=true;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM Alimente;", connection);
                    adapter.Fill(ds, "Alimente");
                    dataGridView1.DataSource = ds.Tables["Alimente"];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Ai dat dublu click pe form!");
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter.SelectCommand.Connection = connection;
                ds.Tables["Alimente"].Clear();
                adapter.Fill(ds, "Alimente");
            }
        }
    }
}
