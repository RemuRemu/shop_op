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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string g_id = textBox1.Text;
            string g_name = textBox2.Text;
            double g_price = Convert.ToDouble(textBox3.Text);
            int g_unit_multip = Convert.ToInt32(numericUpDown1.Value);
            String g_unit = textBox4.Text;
            int g_quantity = Convert.ToInt32(textBox5.Text);


        }
    }
}
