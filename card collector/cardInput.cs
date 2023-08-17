using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace card_collector
{
    public partial class cardInput : Form
    {
        //Connection String for SQL Server Database
        string connectionString = "Server=JAMAL\\SQLEXPRESS02;Database=cards_collection;Integrated Security=true";

        public cardInput()
        {
            InitializeComponent();
        }
        byte[] imageBytes = null;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog=new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.png;*.gif;*.bmp;*.jpeg|All Files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = dialog.FileName;
                // Load the selected image into the PictureBox
                pictureBox1.Image = Image.FromFile(dialog.FileName);
                FileStream fs = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read);
                imageBytes = new byte[fs.Length];
                fs.Read(imageBytes, 0, (int)fs.Length);
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string format = "dddd, MMMM d, yyyy";//Format string
            SqlConnection con=new SqlConnection(connectionString);
            con.Open();
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vendor is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string vendor = textBox1.Text;
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Year is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string year = textBox4.Text;
            if (string.IsNullOrWhiteSpace(textBox9.Text))
            {
                MessageBox.Show("Set is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string set = textBox9.Text;
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("League is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string league = textBox2.Text;
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Team is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string team = textBox5.Text;
            if (string.IsNullOrWhiteSpace(textBox8.Text))
            {
                MessageBox.Show("Player is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string player = textBox8.Text;
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Card Type is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string card_type = textBox3.Text;
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Card Variety is a required field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            string card_variety = textBox6.Text;
            string card_number= textBox7.Text;
            string auto = checkBox1.Checked ? "Yes" : "No";
            string numbered = checkBox2.Checked ? "Yes" : "No";
            string graded = checkBox3.Checked ? "Yes" : "No";
            string grade_company = textBox10.Text;
            DateTime? grade_date = null; // Declare the variable outside of the if block

            if (!string.IsNullOrWhiteSpace(dateTimePicker1.Text))
            {
                grade_date = DateTime.ParseExact(dateTimePicker1.Text, format, System.Globalization.CultureInfo.InvariantCulture);
            }
            string grade = textBox11.Text;
            DateTime? purchase_date = null;
            if (!string.IsNullOrWhiteSpace(dateTimePicker2.Text))
            {
                purchase_date = DateTime.ParseExact(dateTimePicker2.Text, format, System.Globalization.CultureInfo.InvariantCulture);
            }
            string purchase_price=textBox12.Text;
            DateTime? sale_date = null;
            if (!string.IsNullOrWhiteSpace(dateTimePicker3.Text))
            {
                sale_date = DateTime.ParseExact(dateTimePicker3.Text, format, System.Globalization.CultureInfo.InvariantCulture);
            }
            string sale_price = textBox13.Text;
            string quantity = textBox14.Text;

            string query = @"
            INSERT INTO card (
            vendor, [year], [set], league, team, player, cardType, cardVariety, cardNumber, [auto],
            numbered, graded, gradedCompany, GradeDate, grade, purchasedDate, purchasedPrice,
            saleDate, salePrice, imageBlob, quantity
            )
            VALUES (
            @vendor, @year, @set, @league, @team, @player, @cardType, @cardVariety, @cardNumber,
            @auto, @numbered, @graded, @gradedCompany, @gradeDate, @grade, @purchasedDate,
            @purchasedPrice, @saleDate, @salePrice, @imageBlob, @quantity
            );
            ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@vendor", vendor);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@set", set);
            cmd.Parameters.AddWithValue("@league", league);
            cmd.Parameters.AddWithValue("@team", team);
            cmd.Parameters.AddWithValue("@player", player);
            cmd.Parameters.AddWithValue("@cardType", card_type);
            cmd.Parameters.AddWithValue("@cardVariety", card_variety);
            cmd.Parameters.AddWithValue("@cardNumber", card_number);
            cmd.Parameters.AddWithValue("@auto", auto);
            cmd.Parameters.AddWithValue("@numbered", numbered);
            cmd.Parameters.AddWithValue("@graded", graded);
            cmd.Parameters.AddWithValue("@gradedCompany", grade_company);
            if(grade_date.HasValue)
            {
                cmd.Parameters.AddWithValue("@gradeDate", grade_date);
            }
            else
            {
                cmd.Parameters.AddWithValue("@gradeDate", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@grade", grade);
            if (purchase_date.HasValue)
            {
                cmd.Parameters.AddWithValue("@purchasedDate", purchase_date.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@purchasedDate", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@purchasedPrice", purchase_price);
            if(sale_date.HasValue)
            {
                cmd.Parameters.AddWithValue("@saleDate", sale_date);
            }
            else
            {
                cmd.Parameters.AddWithValue("@saleDate", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@salePrice", sale_price);
            if (imageBytes != null && imageBytes.Length > 0)
            {
                cmd.Parameters.AddWithValue("@imageBlob", imageBytes);
            }
            else
            {
                SqlParameter imageParam = new SqlParameter("@imageBlob", SqlDbType.VarBinary);
                imageParam.Value = SqlBinary.Null;
                cmd.Parameters.Add(imageParam);
            }
            cmd.Parameters.AddWithValue("@quantity", quantity);

            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Data Submitted to DataBase", "", MessageBoxButtons.OK);
            Form1.inputTerminator();
        }
            private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
            {
                DateTimePicker dtp = (DateTimePicker)sender;
                dtp.CustomFormat = "";
                dtp.Format = DateTimePickerFormat.Long;
            }
    }
}
