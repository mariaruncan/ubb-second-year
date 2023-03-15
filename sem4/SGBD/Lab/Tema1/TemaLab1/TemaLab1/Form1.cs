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
        DataSet dsParent = new DataSet();
        DataSet dsChild = new DataSet();
        SqlDataAdapter parentAdapter = new SqlDataAdapter();
        SqlDataAdapter childAdapter = new SqlDataAdapter();
        string connectionString = @"Server=DESKTOP-TQEKTP3\SQLEXPRESS;Database=Magazin;Integrated Security=true;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonShowProducatori_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString)) {
                    parentAdapter.SelectCommand = new SqlCommand("SELECT * FROM Producatori;", conn);
                    dsParent.Clear();
                    parentAdapter.Fill(dsParent, "Producatori");
                    dataGridViewParent.DataSource = dsParent.Tables["Producatori"];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void showChildren()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                dsChild.Clear();
                childAdapter.SelectCommand = new SqlCommand("SELECT * FROM Produse WHERE idProducator = @param;", conn);
                childAdapter.SelectCommand.Parameters.Add("@param", SqlDbType.Int).Value = Int32.Parse(textBoxIdProducator.Text);
                childAdapter.Fill(dsChild, "Produse");
                dataGridViewChild.DataSource = dsChild.Tables["Produse"];
            }
        }

        private void dataGridViewParent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                labelIdProdus.Text = "";
                textBoxNume.Text = "";
                textBoxTip.Text = "";
                textBoxVolum.Text = "";
                textBoxPret.Text = "";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (e.RowIndex < dataGridViewParent.RowCount - 1)
                    {
                        string idProducator = dataGridViewParent.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                        textBoxIdProducator.Text = idProducator;
                        textBoxIdProducator.Enabled = false;
                        buttonAdd.Enabled = true;
                        buttonDelete.Enabled = false;
                        buttonUpdate.Enabled = false;

                        showChildren();
                    }
                    else
                    {
                        dsChild.Clear();
                        buttonAdd.Enabled = false;
                        textBoxIdProducator.Text = "";
                        textBoxIdProducator.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    childAdapter.InsertCommand = new SqlCommand("INSERT INTO Produse (nume, tip, volum, pret, idProducator) VALUES (@nume, @tip, @volum, @pret, @idProd);", conn);
                    childAdapter.InsertCommand.Parameters.Add("@nume", SqlDbType.VarChar).Value = textBoxNume.Text;
                    childAdapter.InsertCommand.Parameters.Add("@tip", SqlDbType.VarChar).Value = textBoxTip.Text;
                    childAdapter.InsertCommand.Parameters.Add("@volum", SqlDbType.Int).Value = Int32.Parse(textBoxVolum.Text);
                    childAdapter.InsertCommand.Parameters.Add("@pret", SqlDbType.Money).Value = Convert.ToDecimal(textBoxPret.Text);
                    childAdapter.InsertCommand.Parameters.Add("@idProd", SqlDbType.Int).Value = Int32.Parse(textBoxIdProducator.Text);
                    conn.Open();
                    childAdapter.InsertCommand.ExecuteNonQuery();
                    conn.Close();
                    
                    showChildren();
                    textBoxNume.Text = "";
                    textBoxTip.Text = "";
                    textBoxVolum.Text = "";
                    textBoxPret.Text = "";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridViewChild_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    buttonAdd.Enabled = false;
                    textBoxIdProducator.Enabled = true;
                    if (e.RowIndex < dataGridViewChild.RowCount - 1)
                    {
                        labelIdProdus.Text = dataGridViewChild.Rows[e.RowIndex].Cells[0].Value.ToString();
                        textBoxNume.Text = dataGridViewChild.Rows[e.RowIndex].Cells[1].Value.ToString(); ;
                        textBoxTip.Text = dataGridViewChild.Rows[e.RowIndex].Cells[2].Value.ToString();
                        textBoxVolum.Text = dataGridViewChild.Rows[e.RowIndex].Cells[3].Value.ToString(); ;
                        textBoxPret.Text = dataGridViewChild.Rows[e.RowIndex].Cells[4].Value.ToString(); ;
                        textBoxIdProducator.Text = dataGridViewChild.Rows[e.RowIndex].Cells[5].Value.ToString(); ;
                        buttonDelete.Enabled = true;
                        buttonUpdate.Enabled = true;
                    }
                    else
                    {
                        labelIdProdus.Text = "";
                        textBoxNume.Text = "";
                        textBoxTip.Text = "";
                        textBoxVolum.Text = "";
                        textBoxPret.Text = "";
                        textBoxIdProducator.Text = "";
                        buttonDelete.Enabled = false;
                        buttonUpdate.Enabled = false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    childAdapter.UpdateCommand = new SqlCommand("UPDATE Produse SET nume = @nume, tip = @tip, volum = @volum, pret = @pret, idProducator = @idProducator WHERE idProdus = @idProdus;", conn);
                    childAdapter.UpdateCommand.Parameters.Add("@nume", SqlDbType.VarChar).Value = textBoxNume.Text;
                    childAdapter.UpdateCommand.Parameters.Add("@tip", SqlDbType.VarChar).Value = textBoxTip.Text;
                    childAdapter.UpdateCommand.Parameters.Add("@volum", SqlDbType.Int).Value = Int32.Parse(textBoxVolum.Text);
                    childAdapter.UpdateCommand.Parameters.Add("@pret", SqlDbType.Money).Value = Convert.ToDecimal(textBoxPret.Text);
                    childAdapter.UpdateCommand.Parameters.Add("@idProducator", SqlDbType.Int).Value = Int32.Parse(textBoxIdProducator.Text);
                    childAdapter.UpdateCommand.Parameters.Add("@idProdus", SqlDbType.Int).Value = Int32.Parse(labelIdProdus.Text);
                    conn.Open();
                    childAdapter.UpdateCommand.ExecuteNonQuery();
                    conn.Close();

                    showChildren();
                    textBoxNume.Text = "";
                    textBoxTip.Text = "";
                    textBoxVolum.Text = "";
                    textBoxPret.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    childAdapter.DeleteCommand = new SqlCommand("DELETE FROM Produse WHERE idProdus = @idProdus", conn);
                    childAdapter.DeleteCommand.Parameters.Add("@idProdus", SqlDbType.VarChar).Value = labelIdProdus.Text;
                    conn.Open();
                    childAdapter.DeleteCommand.ExecuteNonQuery();
                    conn.Close();

                    showChildren();
                    labelIdProdus.Text = "";
                    textBoxNume.Text = "";
                    textBoxTip.Text = "";
                    textBoxVolum.Text = "";
                    textBoxPret.Text = "";
                    textBoxIdProducator.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
