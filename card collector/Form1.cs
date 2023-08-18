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
using static System.Net.Mime.MediaTypeNames;

namespace card_collector
{
    public partial class Form1 : Form
    {

        //Connection String for SQL Server Database
        public static string connectionString = "Server=JAMAL\\SQLEXPRESS02;Database=cards_collection;Integrated Security=true";

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        //Input Form Global object
        public static cardInput input = null;
        public static UpdateAndDelete updateForm = null;
        //Static data Table to refresh 
        public static DataTable dataTable = new DataTable();
        public static void inputTerminator()
        {
            input.Close();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "Select * from card";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            dataTable.Clear();
            adapter.Fill(dataTable);
            con.Close();
        }
        public static void updateTerminator()
        {
            updateForm.Close();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "Select * from card";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            dataTable.Clear();
            adapter.Fill(dataTable);
            con.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "Select * from card";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(dataTable);
            dataGridView1.RowTemplate.DefaultCellStyle.Padding = new Padding(0, 0, 0, 10);
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["imageBlob"].Visible = false;
            con.Close();
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input = new cardInput();
            input.Show();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(clickedRow.Cells["id"].Value);
                updateForm = new UpdateAndDelete(id);
                updateForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int idToDelete = Convert.ToInt32(selectedRow.Cells["id"].Value);
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "DELETE FROM card WHERE id = @id;";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idToDelete);
                cmd.ExecuteNonQuery(); // Execute the DELETE query
                string selectQuery = "SELECT * FROM card;";
                SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, con);
                dataTable.Clear();
                adapter.Fill(dataTable);
            }
            else
            {
                MessageBox.Show("Please select a row to delete", "information", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count >= 1)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["id"].Value);
                updateForm = new UpdateAndDelete(id);
                updateForm.Show();
            }
            else
            {
                MessageBox.Show("Please Select a Row", "information", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);


            string searchText = textBox1.Text.Trim().ToLower()+'%'; // Get the entered text and remove any leading/trailing whitespace
            if (searchText.Length > 0) // Ensure there's valid text to search for
            {
                if (checkBox1.Checked)
                {
                    con.Open();
                    string query = "SELECT * FROM card WHERE LOWER(player) LIKE @player";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@player", searchText);
                    dataTable.Clear(); 
                    adapter.Fill(dataTable); 
                    dataGridView1.ClearSelection();
                    con.Close();
                }
                else if (checkBox2.Checked)
                {
                    con.Open();
                    string query = "SELECT * FROM card WHERE LOWER([set]) LIKE @set";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@set", searchText);
                    dataTable.Clear();
                    adapter.Fill(dataTable);
                    dataGridView1.ClearSelection();
                    con.Close();
                }
            }
            else
            {
                con.Open();
                string query = "SELECT * FROM card";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@player", searchText);
                dataTable.Clear(); // Clear the existing data in the DataTable
                adapter.Fill(dataTable); // Fill the DataTable with the new data
                dataGridView1.ClearSelection();
                //rowCount = dataTable.Rows.Count;
                //MessageBox.Show(rowCount == 0 ? "No Cards Found" : rowCount + " Rows Found", "information", MessageBoxButtons.OK);
                con.Close();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                if (checkBox == checkBox1)
                {
                    checkBox2.Checked = false;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }
    }
    
}
