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
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(constr);
            try
            { connection.Open(); }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            string g_id = textBox1.Text;
            string g_name = textBox2.Text;
            double g_price = Convert.ToDouble(textBox3.Text);
            int g_unit_multip = Convert.ToInt32(numericUpDown1.Value);
            string g_unit = textBox4.Text;
            int g_quantity = Convert.ToInt32(textBox5.Text);
            SelectData(g_id);
           if (dataSt.Tables["check"].Rows.Count == 0)
            {
                
                string stmt2 = "insert into Goods (Serial_no,g_name,g_price,g_unitnum,g_unit,g_quantity) VALUES (@g_id,@g_name, @g_price, @g_unit_multip , @g_unit,@g_quantity);";
                SqlCommand cm2 = new SqlCommand(stmt2,connection);

                cm2.Parameters.Clear();
                cm2.Parameters.AddWithValue("g_id", g_id);
                cm2.Parameters.AddWithValue("g_name", g_name);
                cm2.Parameters.AddWithValue("g_price", g_price);
                cm2.Parameters.AddWithValue("g_unit_multip", g_unit_multip);
                cm2.Parameters.AddWithValue("g_unit", g_unit);
                cm2.Parameters.AddWithValue("g_quantity", g_quantity);
                cm2.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show(g_id+g_name+g_quantity);
            }
            else {
                int db_quantity = Convert.ToInt32(dataSt.Tables["check"].Rows[0]["g_quantity"]);
                int new_quantity = db_quantity + g_quantity;
                string stmt2 = "UPDATE Goods SET g_quantity = @new_quantity WHERE Serial_no = @g_id;";
                SqlCommand cm2 = new SqlCommand(stmt2, connection);
                cm2.Parameters.Clear();
                cm2.Parameters.AddWithValue("new_quantity", new_quantity);
                cm2.Parameters.AddWithValue("g_id", g_id);
                cm2.ExecuteNonQuery();
                connection.Close();
            }
        }
        private void SelectData(String g_id){
           
            string stmt = "Select * from Goods where (Serial_no = @g_id);";
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
