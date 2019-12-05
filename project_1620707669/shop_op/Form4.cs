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
            try // try open connection
            {
                connection.Open();
            }
            catch(Exception Ex) {
                MessageBox.Show(Ex.ToString());
            }

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            
            string Serial_no = textBox1.Text;
            int multiply_good = 1;
            findGoods(Serial_no); //หาสินค้าจาก Serial NO
            if (radioButton1.Checked) { multiply_good = 1; }
            else if (radioButton2.Checked) { multiply_good = 2; }
            else if (radioButton3.Checked) { multiply_good = 3; }
            else if (radioButton4.Checked) { multiply_good = 4; }
            if (dataSt.Tables["checkout"].Rows.Count == 0) // ถ้าตาราง checkout ไมมีข้อมูล ให้แจ้ง "Serial NO ไม่ถูกต้อง"
            {
                MessageBox.Show("Serial NO ไม่ถูกต้อง");
            }
            else {
                //สร้าง Object ของ Goods เพื่อเก็บค่าที่ได้จากตาราง
                Goods good = new Goods();
                good.Serial_no1 = dataSt.Tables["checkout"].Rows[0]["Serial_no"].ToString();
                good.G_name = dataSt.Tables["checkout"].Rows[0]["g_name"].ToString();
                good.G_price = Convert.ToDouble(dataSt.Tables["checkout"].Rows[0]["g_price"]);
                good.G_unit = dataSt.Tables["checkout"].Rows[0]["g_unit"].ToString();
                good.G_unitnum = dataSt.Tables["checkout"].Rows[0]["g_unitnum"].ToString();
                good.G_quantity = multiply_good;
                //นำ Object Good เก็บลงใน List ที่ชื่อว่า Cart
                cart.Add(good); 
                // คำนวนราคาทั้งหมด
                total_price += good.G_price*multiply_good;
                // แสดงข้อมูลสืนค้าลงใน listBox1
                string addlistbox = good.Serial_no1 +" "+ good.G_name+" "+good.G_unitnum+good.G_unit+" "+ good.G_price+"bath@"+multiply_good;
                listBox1.Items.Add(addlistbox);
                //แสดงราคาทั้งหมดลงใน lable2
                label2.Text ="ราคาทั้งหมด : "+total_price+" บาท";
            }           
        }

        private void findGoods(string Serial_no) {
            // ค้นหาสินค้าด้วย Serial_no 
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
                //iscustomer มี default เป็น false
                // เช็คว่าเป็นลูกค้าหรือไม่ ถ้าเป็นก็จะนำราคาสินค้ามาคำนวนหาคะแนนสะสมให้ลูกค้า
                int c_point = Convert.ToInt32(dataSt.Tables["customers"].Rows[0]["c_point"]) + (int)Math.Floor(total_price);
                string stmt = "UPDATE Customers SET c_point = @c_point WHERE c_no = @c_no";
                SqlCommand cm = new SqlCommand(stmt, connection);
                cm.Parameters.Clear();
                cm.Parameters.AddWithValue("@c_point", c_point);
                cm.Parameters.AddWithValue("c_no", dataSt.Tables["customers"].Rows[0]["c_no"].ToString());
                cm.ExecuteNonQuery();
  
                label6.Text = "ชื่อสมาชิก : " + dataSt.Tables["customers"].Rows[0]["c_name"].ToString();
                label7.Text = "รหัสสมาชิก : " + dataSt.Tables["customers"].Rows[0]["c_no"].ToString();
                label8.Text = dataSt.Tables["customers"].Rows[0]["c_address"].ToString();
                label9.Text = "คะแนนสะสม : " + c_point;
            }
            
            for (int i = 0; i < cart.Count; i++) {
                //นำสินค้าจาก List cart ออกมาทีละชิ้น
                Goods goods = cart[i];
                // findGoods จาก Serial No เพื่อจะเอาค่าปัจจุบันของ g_quantity ซึ่งเป็นจำนวนสินค้าใน stock ของสินค้าชิ้นนั้น
                // มาหักจากค่าที่เก็บมาใน list ทีละชิ้นจนครบจำนวนใน list
                findGoods(goods.Serial_no1);
                string stmt2 = "UPDATE Goods SET g_quantity= @g_quantity WHERE Serial_no = @Serial_no";
                int new_quantity = Convert.ToInt32(dataSt.Tables["checkout"].Rows[0]["g_quantity"]) - goods.G_quantity;
                SqlCommand cm2 = new SqlCommand(stmt2, connection);
                cm2.Parameters.Clear();
                cm2.Parameters.AddWithValue("g_quantity", new_quantity);
                cm2.Parameters.AddWithValue("Serial_no", goods.Serial_no1);
                cm2.ExecuteNonQuery();
                listBox3.Items.Add(goods.G_quantity);
                listBox2.Items.Add(goods.Serial_no1 + " "+goods.G_name);
                listBox5.Items.Add(goods.G_unitnum + " " + goods.G_unit);
                listBox4.Items.Add(goods.G_price);
                listBox6.Items.Add(goods.G_price * goods.G_quantity);
                label10.Text = "เวลา : "+DateTime.Now.ToString();
            }
            MessageBox.Show("ชำระสินค้าเรียบร้อยแล้ว");
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            
            string c_no = textBox2.Text;
            // ค้นหาสมาชิกจาก รหัสสมาชิก หรือ เบอร์โทรศัพท์ อย่างใดอย่างหนึ่ง
            findCustomers(c_no);
            if (dataSt.Tables["customers"].Rows.Count == 0) // ถ้าไม่มีข้อมูลใน customers แสดง  รหัสสมาชิก/เบอร์โทรศัพท์ไม่ถูกต้อง
            {
                MessageBox.Show("รหัสสมาชิก/เบอร์โทรศัพท์ไม่ถูกต้อง");
            }
            else {
                // ถ้าเป็นสมาชิก เปลี่ยนค่า iscustomer จาก false เป็น true
                // แสดงชื่อและที่อยู่ของลูกค้า
                iscustomer = true;
                label4.Text = dataSt.Tables["customers"].Rows[0]["c_name"].ToString();
                label5.Text = dataSt.Tables["customers"].Rows[0]["c_address"].ToString();
            }
        }
        private void findCustomers(string c_no) {
            // หาลูกค้าด้วยรหัสสมาชิกหรือเบอร์โทรศัพท์
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
