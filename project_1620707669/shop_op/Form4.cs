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
    public partial class Form4 : Form
    {
        private SqlConnection connection;
        private DataSet dataSt;
        private IList<Goods> cart = new List<Goods>();
        private double total_price;
        private Boolean iscustomer = false;
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
            connection = new SqlConnection(conStr);
            connection.Open();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            
            string Serial_no = textBox1.Text;
            int multiply_good = 1;
            findGoods(Serial_no);
            if (radioButton1.Checked) { multiply_good = 1; }
            else if (radioButton2.Checked) { multiply_good = 2; }
            else if (radioButton3.Checked) { multiply_good = 3; }
            else if (radioButton4.Checked) { multiply_good = 4; }
            if (dataSt.Tables["checkout"].Rows.Count == 0)
            {
                MessageBox.Show("Serial NO ไม่ถูกต้อง");
            }
            else {
                Goods good = new Goods();
                good.Serial_no1 = dataSt.Tables["checkout"].Rows[0]["Serial_no"].ToString();
                good.G_name = dataSt.Tables["checkout"].Rows[0]["g_name"].ToString();
                good.G_price = Convert.ToDouble(dataSt.Tables["checkout"].Rows[0]["g_price"]);
                good.G_unit = dataSt.Tables["checkout"].Rows[0]["g_unit"].ToString();
                good.G_unitnum = dataSt.Tables["checkout"].Rows[0]["g_unitnum"].ToString();
                good.G_quantity = multiply_good;
                cart.Add(good);
                total_price += good.G_price*multiply_good;
                string addlistbox = good.Serial_no1 +" "+ good.G_name+" "+good.G_unitnum+good.G_unit+" "+ good.G_price+"bath@"+multiply_good;
                listBox1.Items.Add(addlistbox);
                label2.Text ="ราคาทั้งหมด : "+total_price+" บาท";
            }           
        }

        private void findGoods(string Serial_no) {
            string stmt = "SELECT * FROM Goods WHERE Serial_no = @Serial_no";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.Parameters.Clear();
            cm.Parameters.AddWithValue("@Serial_no", Serial_no);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "checkout");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (iscustomer == true) {
                int c_point = Convert.ToInt32(dataSt.Tables["customers"].Rows[0]["c_point"]) + (int)Math.Floor(total_price);
                string stmt = "UPDATE Customers SET c_point = @c_point WHERE c_no = @c_no";
                SqlCommand cm = new SqlCommand(stmt, connection);
                cm.Parameters.Clear();
                cm.Parameters.AddWithValue("@c_point", c_point);
                cm.Parameters.AddWithValue("c_no", dataSt.Tables["customers"].Rows[0]["c_no"].ToString());
                cm.ExecuteNonQuery();
                MessageBox.Show(c_point.ToString() + dataSt.Tables["customers"].Rows[0]["c_no"].ToString()); ;

            }
            for (int i = 0; i < cart.Count; i++) {
                Goods goods = cart[i];
                findGoods(goods.Serial_no1);
                string stmt2 = "UPDATE Goods SET g_quantity= @g_quantity WHERE Serial_no = @Serial_no";
                int new_quantity = Convert.ToInt32(dataSt.Tables["checkout"].Rows[0]["g_quantity"]) - goods.G_quantity;
                SqlCommand cm2 = new SqlCommand(stmt2, connection);
                cm2.Parameters.Clear();
                cm2.Parameters.AddWithValue("g_quantity", new_quantity);
                cm2.Parameters.AddWithValue("Serial_no", goods.Serial_no1);
                
                cm2.ExecuteNonQuery();
                MessageBox.Show(goods.Serial_no1+" "+new_quantity);
            }
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string c_no = textBox2.Text;
            findCustomers(c_no);
            if (dataSt.Tables["customers"].Rows.Count == 0)
            {
                MessageBox.Show("รหัสสมาชิก/เบอร์โทรศัพท์ไม่ถูกต้อง");
            }
            else {
                iscustomer = true;
                label4.Text = dataSt.Tables["customers"].Rows[0]["c_name"].ToString();
                label5.Text = dataSt.Tables["customers"].Rows[0]["c_address"].ToString();
            }
        }
        private void findCustomers(string c_no) {
            string stmt = "SELECT * FROM Customers where c_no = @customers or c_phone = @customers";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.Parameters.Clear();
            cm.Parameters.AddWithValue("@customers", c_no);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "customers");

        }
    }
}
