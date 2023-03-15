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

namespace Seminar_2_226_SGBD
{
    public partial class Form1 : Form
    {
        DataSet dataset = new DataSet();
        SqlDataAdapter parentAdapter = new SqlDataAdapter();
        SqlDataAdapter childAdapter = new SqlDataAdapter();
        string connectionString = @"Server=DESKTOP-TQEKTP3\SQLEXPRESS;Database=Seminar2226SGBD;Integrated Security=true;";
        BindingSource bsParent = new BindingSource();
        BindingSource bsChild = new BindingSource();  

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
                    parentAdapter.SelectCommand = new SqlCommand("SELECT * FROM Orase;", connection);
                    childAdapter.SelectCommand = new SqlCommand("SELECT * FROM Cartiere;", connection);
                    parentAdapter.Fill(dataset, "Orase");
                    childAdapter.Fill(dataset, "Cartiere");
                    DataColumn pkColumn = dataset.Tables["Orase"].Columns["cod_o"];
                    DataColumn fkColumn = dataset.Tables["Cartiere"].Columns["cod_o"];
                    DataRelation relation = new DataRelation("fk_Orase_Cartiere", pkColumn, fkColumn);
                    dataset.Relations.Add(relation);
                    bsParent.DataSource = dataset.Tables["Orase"];  
                    dataGridViewParent.DataSource = bsParent;
                    labelParent.Text = "Orase";

                    bsChild.DataSource = bsParent;
                    bsChild.DataMember = "fk_Orase_Cartiere";
                    dataGridViewChild.DataSource = bsChild;
                    labelChild.Text = "Cartiere";

                    textBox1.DataBindings.Add("Text", bsParent, "Nume");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
