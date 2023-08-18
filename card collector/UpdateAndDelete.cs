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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace card_collector
{
    public partial class UpdateAndDelete : Form
    {
        private int id;
        public UpdateAndDelete(int id)
        {
            InitializeComponent();
            this.id= id;
        }
        byte[] imageBytes = null;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
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
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.CustomFormat = "";
            dtp.Format = DateTimePickerFormat.Long;
        }
        //Connection String
        public static string connectionString = "Server=JAMAL\\SQLEXPRESS02;Database=cards_collection;Integrated Security=true";

        private void UpdateAndDelete_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string query = "SELECT * FROM Card WHERE id = @id";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id); 
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string vendor = reader["vendor"] != DBNull.Value ? reader["vendor"].ToString() : "";
                string year = reader["year"] != DBNull.Value  ? reader["year"].ToString() : "";
                string set = reader["set"] != DBNull.Value ? reader["set"].ToString() : "";
                string league = reader["league"] != DBNull.Value ? reader["league"].ToString() : "";
                string team = reader["team"] != DBNull.Value  ? reader["team"].ToString() : "";
                string player = reader["player"] != DBNull.Value ? reader["player"].ToString() : "";
                string card_type = reader["cardType"] != DBNull.Value ? reader["cardType"].ToString() : "";
                string card_variety = reader["cardVariety"] != DBNull.Value ? reader["cardVariety"].ToString() : "";
                string card_number = reader["cardNumber"] != DBNull.Value ? reader["cardNumber"].ToString() : "";
                string auto = reader["auto"] != DBNull.Value ? reader["auto"].ToString() : "";
                string numbered = reader["numbered"] != DBNull.Value  ? reader["numbered"].ToString() : "";
                string graded = reader["graded"] != DBNull.Value  ? reader["graded"].ToString() : "";
                string graded_company = reader["gradedCompany"] != DBNull.Value  ? reader["gradedCompany"].ToString() : "";


                DateTime grade_date = DateTime.MinValue;
                if (reader["gradeDate"] != DBNull.Value || reader["gradeDate"].ToString()!="")
                {
                    grade_date = (DateTime)reader["gradeDate"];
                }
                string grade = reader["grade"].ToString();
                DateTime purchase_date = DateTime.MinValue;
                if (reader["purchasedDate"] != DBNull.Value)
                {
                    purchase_date = (DateTime)reader["purchasedDate"];
                }
                string purchased_price = reader["purchasedPrice"].ToString();

                DateTime saled_date = DateTime.MinValue;
                if (reader["saleDate"] != DBNull.Value && reader["saleDate"] != "")
                {
                    saled_date = (DateTime)reader["saleDate"];

                }
                string saled_price = reader["salePrice"].ToString();
                byte[] imageBytes = null;
                Image image = null;
                if (!reader.IsDBNull(reader.GetOrdinal("imageBlob")))
                {
                    imageBytes = (byte[])reader["imageBlob"];
                    // Convert the image bytes to an Image object
                    MemoryStream ms = new MemoryStream(imageBytes);
                    image = Image.FromStream(ms);
                    
                }
                int quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                
                textBox1.Text = vendor;
                textBox4.Text = year;
                textBox9.Text = set;
                textBox2.Text = league;
                textBox5.Text = team;
                textBox8.Text = player;
                textBox3.Text = card_type;
                textBox6.Text = card_variety;
                textBox7.Text = card_number;
                if (auto == "Yes")
                {
                    checkBox1.Checked = true;
                }
                if (numbered == "Yes")
                {
                    checkBox2.Checked = true;
                }
                if (graded == "Yes")
                {
                    checkBox3.Checked = true;
                }
                textBox14.Text = quantity.ToString();
                textBox10.Text = graded_company;
                if (grade_date != DateTime.MinValue)
                {
                    dateTimePicker1.Value = grade_date;

                }
                textBox11.Text = grade;
                if(purchase_date!=DateTime.MinValue)
                {
                    dateTimePicker2.Value = purchase_date;
                }
                textBox12.Text = purchased_price;
                if (imageBytes != null)
                {
                    pictureBox1.Image = image;
                }
                if(saled_date!=DateTime.MinValue)
                {
                    dateTimePicker3.Value = saled_date;
                }
                textBox13.Text = saled_price;
            }
            reader.Close();
            con.Close();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            string format = "dddd, MMMM d, yyyy";//Format string
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string vendor = textBox1.Text;
            string year = textBox4.Text;
            string set = textBox9.Text;
            string league = textBox2.Text;
            string team = textBox5.Text;
            string player = textBox8.Text;
            string card_type = textBox3.Text;
            string card_variety = textBox6.Text;
            string card_number = textBox7.Text;
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
            string purchase_price = textBox12.Text;
            DateTime? sale_date = null;
            if (!string.IsNullOrWhiteSpace(dateTimePicker3.Text))
            {
                sale_date = DateTime.ParseExact(dateTimePicker3.Text, format, System.Globalization.CultureInfo.InvariantCulture);
            }
            string sale_price = textBox13.Text;
            int quantity=1;
            if (int.TryParse(textBox14.Text, out int result))
            {
                quantity = result;
            }
            else
            {
                con.Close();
                MessageBox.Show("Quantity can only be integer", "warning", MessageBoxButtons.OK);
                return;
            }

            string query = @"
            UPDATE card SET
            vendor = @vendor, [year] = @year, [set] = @set, league = @league, team = @team, player = @player,
            cardType = @cardType, cardVariety = @cardVariety, cardNumber = @cardNumber, [auto] = @auto,
            numbered = @numbered, graded = @graded, gradedCompany = @gradedCompany, GradeDate = @gradeDate,
            grade = @grade, purchasedDate = @purchasedDate, purchasedPrice = @purchasedPrice,
            saleDate = @saleDate, salePrice = @salePrice, imageBlob = @imageBlob, quantity = @quantity
            WHERE id = @id;
            ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
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
            if (grade_date.HasValue)
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
            if (sale_date.HasValue)
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

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                MessageBox.Show("A record with this value already exists.", "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                con.Close();
                return;
            }
            con.Close();
            MessageBox.Show("Data Updated", "", MessageBoxButtons.OK);
            Form1.updateTerminator();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            Form1.updateTerminator();
        }
    }
}
