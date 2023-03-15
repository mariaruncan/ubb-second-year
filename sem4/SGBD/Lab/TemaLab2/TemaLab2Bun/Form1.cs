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
using System.Configuration;
using System.Xml;
using TemaLab1;
using System.Collections;

namespace Seminar_2_226_SGBD
{
    internal partial class Form1 : Form
    {
        DataSet dsParent = new DataSet();
        DataSet dsChild = new DataSet();
        SqlDataAdapter parentAdapter = new SqlDataAdapter();
        SqlDataAdapter childAdapter = new SqlDataAdapter();
        string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        Table parentTable, childTable;
        List<Label> labelsList = new List<Label>();
        List<TextBox> textboxesList = new List<TextBox>();
        List<string> pksParent = new List<string>();
        List<string> fksChild = new List<string>();

        public Form1(Table parent, Table child)
        {
            this.parentTable = parent;
            this.childTable = child;
            InitializeComponent();

            //
            // My code for initialize
            //
            this.MaximumSize = this.MinimumSize = this.Size;
            int y = 50;
            for (int i = 0; i < child.NoFields; i++)
            {
                string nameColumn = child.Columns[i].Name;
                TextBox tb = new TextBox();
                tb.Location = new Point(800, y);
                tb.Name = "textbox" + nameColumn;
                tb.Size = new Size(196, 22);
                textboxesList.Add(tb);
                this.Controls.Add(tb);
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Location = new Point(720, y);
                lb.Name = "label" + nameColumn;
                lb.Size = new Size(44, 16);
                lb.Text = nameColumn;
                labelsList.Add(lb);
                this.Controls.Add(lb);
                y += 30;
            }

            foreach (Column c in parent.Columns)
                if (c.IsPK)
                    pksParent.Add(c.Name);
            foreach (Column c in child.Columns)
                if (c.IsFK)
                    fksChild.Add(c.Name);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonShowParentContent_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    parentAdapter.SelectCommand = new SqlCommand(parentTable.SelectCmd, conn);
                    dsParent.Clear();
                    parentAdapter.Fill(dsParent, parentTable.Name);
                    dataGridViewParent.DataSource = dsParent.Tables[parentTable.Name];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void showChildren()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                dsChild.Clear();
                childAdapter.SelectCommand = new SqlCommand(childTable.SelectCmd, conn);
                int i = 0;
                foreach (string name in fksChild)
                {
                    TextBox tb = textboxesList.Where(x => x.Name == "textbox" + name).First();
                    childAdapter.SelectCommand.Parameters.AddWithValue("@param" + i.ToString(), tb.Text);
                    i++;
                }
                childAdapter.Fill(dsChild, childTable.Name);
                dataGridViewChild.DataSource = dsChild.Tables[childTable.Name];
            }
        }

        private void dataGridViewParent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (TextBox tb in textboxesList)
                {
                    tb.Text = "";
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (e.RowIndex < dataGridViewParent.RowCount - 1)
                    {
                        for (int i = 0; i < pksParent.Count; i++)
                        {
                            // ma bazez ca coloanele din pk in parinte apar in aceeasi ordine in fk in copil
                            string value = dataGridViewParent.Rows[e.RowIndex].Cells[pksParent[i]].FormattedValue.ToString();
                            TextBox tp = textboxesList.Find(x => x.Name == "textbox" + fksChild[i]);
                            tp.Text = value;
                            tp.Enabled = false;
                        }
                        buttonAdd.Enabled = true;
                        buttonDelete.Enabled = false;
                        buttonUpdate.Enabled = false;

                        showChildren();
                    }
                    else
                    {
                        dsChild.Clear();
                        buttonAdd.Enabled = false;
                        foreach (string name in fksChild)
                        {
                            TextBox tp = textboxesList.Find(x => x.Name == name);
                            tp.Text = "";
                            tp.Enabled = false;
                        }
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    childAdapter.InsertCommand = new SqlCommand(childTable.InsertCmd, conn);
                    List<string> notPksChild = childTable.Columns.FindAll(x => !x.IsPK).ConvertAll(x => x.Name);
                    int i = 0;
                    foreach (string colName in notPksChild)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        childAdapter.InsertCommand.Parameters.AddWithValue("@param" + i.ToString(), tp.Text);
                        i++;
                    }
                    conn.Open();
                    childAdapter.InsertCommand.ExecuteNonQuery();
                    conn.Close();

                    showChildren();

                    List<string> notPksOrFksChild = childTable.Columns.FindAll(x => !x.IsFK && !x.IsPK).ConvertAll(x => x.Name);
                    foreach (string colName in notPksOrFksChild)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        tp.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridViewChild_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    buttonAdd.Enabled = false;
                    foreach (string colName in fksChild)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        tp.Enabled = true;
                    }
                    if (e.RowIndex < dataGridViewChild.RowCount - 1)
                    {
                        foreach (string colName in childTable.Columns.ConvertAll(x => x.Name))
                        {
                            TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                            tp.Text = dataGridViewChild.Rows[e.RowIndex].Cells[colName].Value.ToString();
                        }
                        buttonDelete.Enabled = true;
                        buttonUpdate.Enabled = true;
                    }
                    else
                    {
                        foreach (TextBox tp in textboxesList)
                            tp.Text = "";
                        buttonDelete.Enabled = false;
                        buttonUpdate.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
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
                    childAdapter.UpdateCommand = new SqlCommand(childTable.UpdateCmd, conn);
                    List<string> notPksChild = childTable.Columns.FindAll(x => !x.IsPK).ConvertAll(x => x.Name);
                    List<string> pksChild = childTable.Columns.FindAll(x => x.IsPK).ConvertAll(x => x.Name);

                    int i = 0;
                    foreach (string colName in notPksChild)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        childAdapter.UpdateCommand.Parameters.AddWithValue("@param" + i.ToString(), tp.Text);
                        i++;
                    }
                    foreach (string colName in pksChild)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        childAdapter.UpdateCommand.Parameters.AddWithValue("@param" + i.ToString(), tp.Text);
                        i++;
                    }
                    conn.Open();
                    childAdapter.UpdateCommand.ExecuteNonQuery();
                    conn.Close();

                    showChildren();
                    foreach (string colName in childTable.Columns.FindAll(x => !x.IsFK && !x.IsPK).ConvertAll(x => x.Name))
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + colName);
                        tp.Text = "";
                    }
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
                    childAdapter.DeleteCommand = new SqlCommand(childTable.DeleteCmd, conn);
                    List<string> pksChild = childTable.Columns.FindAll(x => x.IsPK).ConvertAll(x => x.Name);
                    for (int i = 0; i < pksChild.Count; i++)
                    {
                        TextBox tp = textboxesList.Find(x => x.Name == "textbox" + pksChild[i]);
                        childAdapter.DeleteCommand.Parameters.AddWithValue("@param" + i.ToString(), tp.Text);
                    }
                    conn.Open();
                    childAdapter.DeleteCommand.ExecuteNonQuery();
                    conn.Close();

                    showChildren();
                    foreach (TextBox tp in textboxesList)
                        tp.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
