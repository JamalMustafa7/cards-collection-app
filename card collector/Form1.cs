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

        }

        //Input Form Global object
        public static cardInput input =null;
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
            SqlConnection con=new SqlConnection(connectionString);
            con.Open();
            string query = "Select * from card";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);          
            adapter.Fill(dataTable);
            dataGridView1.RowTemplate.DefaultCellStyle.Padding = new Padding(0, 0, 0,10);
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["imageBlob"].Visible = false;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input=new cardInput();
            input.Show();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];
                int vendor = Convert.ToInt32(clickedRow.Cells["id"].Value);
                updateForm= new UpdateAndDelete(vendor);
                updateForm.Show();
            }

        }
    }
}
