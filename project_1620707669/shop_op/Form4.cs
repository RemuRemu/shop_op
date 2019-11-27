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
                cart.Add(good);
                total_price += good.G_price*multiply_good;
                string addlistbox = good.Serial_no1 +" "+ good.G_name+" "+good.G_unit+good.G_unitnum+" "+ good.G_price+"@"+multiply_good;
                listBox1.Items.Add(addlistbox);
                label2.Text ="ราคาทั้งหมด : "+total_price;
            }           
        }

        private void findGoods(string Serial_no) {
            string stmt = "SELECT * FROM Goods where Serial_no = @Serial_no";
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

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string c_no = textBox2.Text;
            findCustomers(c_no);
            if (dataSt.Tables["checkout"].Rows.Count == 0)
            {
                MessageBox.Show("รหัสสมาชิก/เบอร์โทรศัพท์ไม่ถูกต้อง");
            }
            else {
                
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
