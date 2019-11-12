using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shop_op
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox4.Enabled = true;
            }
            else {
                textBox4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String c_name = textBox1.Text;
            String c_address = textBox2.Text;
            String c_email = textBox3.Text;
            String c_phone = textBox5.Text;
            if (radioButton1.Checked) { String c_person = radioButton1.Text; }
            else if (radioButton2.Checked) { String c_person = radioButton2.Text; }
            else if (radioButton3.Checked) { String c_person = textBox4.Text; }
        }
    }
}
