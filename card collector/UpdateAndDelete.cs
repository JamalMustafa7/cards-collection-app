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
    public partial class UpdateAndDelete : Form
    {
        public UpdateAndDelete()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
               
        }
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.CustomFormat = "";
            dtp.Format = DateTimePickerFormat.Long;
            MessageBox.Show(dtp.Value.ToString(), " ", MessageBoxButtons.OK);
        }
    }
}
