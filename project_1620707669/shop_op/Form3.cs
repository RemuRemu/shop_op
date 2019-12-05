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
    public partial class Form3 : Form
    {
        private SqlConnection connection;
        private DataSet dataSt;
        public Form3()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            String c_no = textBox4.Text;
            String c_name = textBox1.Text;
            String c_address = textBox2.Text;
            String c_email = textBox3.Text;
            String c_phone = textBox5.Text;
            findCustomers(c_no);
            if (dataSt.Tables["customers"].Rows.Count == 0) // ถ้าไม่มีข้อมูลในตารางแปลว่า บัตรนี้สามารถใช้งานได้ และทำการสมัครสมาชิก
            {
                string stmt = "INSERT INTO Customers VALUES(@c_no,@c_name,@c_address,@c_email,@c_phone,0)";
                SqlCommand cm = new SqlCommand(stmt,connection);
                cm.Parameters.AddWithValue("c_no",c_no);
                cm.Parameters.AddWithValue("c_name", c_name);
                cm.Parameters.AddWithValue("c_address", c_address);
                cm.Parameters.AddWithValue("c_email", c_email);
                cm.Parameters.AddWithValue("c_phone", c_phone);
                cm.ExecuteNonQuery();
                findCustomers();
                dataGridView1.DataSource = dataSt.Tables["lcustomers"];
                MessageBox.Show("สมัครสมาชิกใหม่เรียบร้อยแล้ว");
            }
            else { MessageBox.Show("รหัสบัตรนี้มีผู้ใช้งานแล้ว"); }
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet1.Customers' table. You can move, or remove it, as needed.
            this.customersTableAdapter.Fill(this.database1DataSet1.Customers);
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
            connection = new SqlConnection(constr);
            try // try open connection
            {
                connection.Open();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void findCustomers(string c_no)
        {
            // ตรวจสอบรหัสบัตรจาก DB ด้วยรหัสของบัตรสมาชิก
            string stmt = "SELECT * FROM Customers where c_no = @customers or c_phone = @customers";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.Parameters.Clear();
            cm.Parameters.AddWithValue("@customers", c_no);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "customers");

        }
        private void findCustomers()
        {
            // นำค่าจากตารางเตรียมแก้ไข dataGridView
            string stmt = "SELECT * FROM Customers";
            SqlCommand cm = new SqlCommand(stmt, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            cm.ExecuteNonQuery();
            dataSt = new DataSet();
            adapter.Fill(dataSt, "lcustomers");

        }

    }
}
