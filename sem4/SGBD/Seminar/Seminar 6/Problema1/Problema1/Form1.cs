using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Problema1
{
    public partial class Form1 : Form
    {
        private DataSet dataSet = new DataSet();
        private SqlDataAdapter cofetarieAdapter = new SqlDataAdapter();
        private SqlDataAdapter briosaAdapter = new SqlDataAdapter();
        string connectionString = @"Server=DESKTOP-TQEKTP3\SQLEXPRESS;Database=Problema1;Integrated Security=true;";
        private BindingSource bsBriosa = new BindingSource();
        private BindingSource bsCofetarie = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Comenzi
                    cofetarieAdapter.SelectCommand = new SqlCommand("SELECT * FROM Cofetarii;", conn);
                    briosaAdapter.SelectCommand = new SqlCommand("SELECT * FROM Briose;", conn);


                    // Fill data source
                    cofetarieAdapter.Fill(dataSet, "Cofetarii");
                    briosaAdapter.Fill(dataSet, "Briose");

                    // Setare relatie fk
                    DataColumn pkColumn = dataSet.Tables["Cofetarii"].Columns["cod_cofetarie"];
                    DataColumn fkColumn = dataSet.Tables["Briose"].Columns["cod_cofetarie"];
                    DataRelation dataRelation = new DataRelation("fk_cofetarii_briose", pkColumn, fkColumn);
                    dataSet.Relations.Add(dataRelation);

                    // 
                    bsCofetarie.DataSource = dataSet.Tables["Cofetarii"];
                    bsBriosa.DataSource = bsCofetarie;
                    bsBriosa.DataMember = "fk_cofetarii_briose";

                    // 
                    dataGridViewBriose.DataSource = bsBriosa;

                    listBoxCofetarii.DataSource = bsCofetarie;
                    listBoxCofetarii.DisplayMember = "nume_cofetarie";
                    listBoxCofetarii.ValueMember = "cod_cofetarie";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            // Update
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    briosaAdapter.SelectCommand.Connection = conn;
                    SqlCommandBuilder builder = new SqlCommandBuilder(briosaAdapter);
                    briosaAdapter.Update(dataSet, "Briose");
                    dataSet.Tables["Briose"].Clear();
                    briosaAdapter.Fill(dataSet, "Briose");
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
