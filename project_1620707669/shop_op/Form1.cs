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
namespace shop_op
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection connection;
        private DataSet dataSt;
        String constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
       

        private void button1_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(constr);
            connection.Open();
            string g_id = textBox1.Text;
            string g_name = textBox2.Text;
            double g_price = Convert.ToDouble(textBox3.Text);
            int g_unit_multip = Convert.ToInt32(numericUpDown1.Value);
            String g_unit = textBox4.Text;
            int g_quantity = Convert.ToInt32(textBox5.Text);
            SelectData(g_id);
           if (dataSt.Tables["check"] == null)
            {
                String stmt2 = "insert into Goods VALUES (@g_id,@g_name, @g_price, @g_unit_multip , @g_unit,@g_quantity);";
                SqlCommand cm = new SqlCommand(stmt2, connection);

                cm.Parameters.Clear();
                cm.Parameters.AddWithValue("g_id", g_id);
                cm.Parameters.AddWithValue("g_name", g_name);
                cm.Parameters.AddWithValue("g_price", g_price);
                cm.Parameters.AddWithValue("g_unit_multip", g_unit_multip);
                cm.Parameters.AddWithValue("g_unit", g_unit);
                cm.Parameters.AddWithValue("g_quantity", g_quantity);
                cm.ExecuteNonQuery();
                MessageBox.Show("Yatta!");
            }
            else {
                
            }
        }
        private void SelectData(String g_id){
            MessageBox.Show("checkA");
            String stmt = "Select * from Goods where Serial_no = @g_id";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.Parameters.Clear();
            cm.Parameters.AddWithValue("g_id", g_id);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "check");
        }
    }
}
